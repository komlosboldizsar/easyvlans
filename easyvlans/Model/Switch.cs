using easyvlans.Helpers;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    public class Switch
    {

        public readonly string ID;
        public readonly string Label;
        private readonly IPEndPoint ipEndPoint;
        private readonly OctetString readCommunity;
        private readonly OctetString writeCommunity;

        private List<UserPort> ports = new();
        private List<UserPort> portsWithPendingChange = new();

        private Config config;

        public delegate void PortsWithPendingChangeCountChangedDelegate(Switch @switch, int newValue);
        public event PortsWithPendingChangeCountChangedDelegate PortsWithPendingChangeCountChanged;
        private int _portsWithPendingChangeCount;
        public int PortsWithPendingChangeCount
        {
            get => _portsWithPendingChangeCount;
            private set
            {
                if (value == _portsWithPendingChangeCount)
                    return;
                _portsWithPendingChangeCount = value;
                PortsWithPendingChangeCountChanged?.Invoke(this, value);
            }
        }

        public delegate void StatusChangedDelegate(Switch @switch, SwitchStatus newValue);
        public event StatusChangedDelegate StatusChanged;
        private SwitchStatus _status = SwitchStatus.NotConnected;
        public SwitchStatus Status
        {
            get => _status;
            private set
            {
                if (value == _status)
                    return;
                _status = value;
                StatusChanged?.Invoke(this, value);
            }
        }

        public Switch(string id, string label, string ip, int port, string readCommunity, string writeCommunity)
        {
            ID = id;
            Label = label;
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            this.readCommunity = new OctetString(readCommunity);
            this.writeCommunity = new OctetString(writeCommunity);
        }

        internal void AssignConfig(Config config) => this.config = config;

        internal void AssociatePort(UserPort port)
        {
            if ((port.Switch == this) && !ports.Contains(port))
                ports.Add(port);
        }

        public async Task ReadConfigAsync()
        {
            Dictionary<int, SnmpVlan> snmpVlans = await ReadSnmpVlansAsync();
            Dictionary<int, SnmpPort> snmpPorts = await ReadSnmpPortsAsync();
            BindUserToSnmpVlans(snmpVlans);
            CalculateSnmpPortVlanMemberships(snmpVlans, snmpPorts);
        }

        private async Task<List<Variable>> SnmpBulkWalkAsync(string objectIdentifierStr)
        {
            List<Variable> result = new List<Variable>();
            await Messenger.BulkWalkAsync(VersionCode.V2, ipEndPoint, readCommunity, OctetString.Empty,
                new ObjectIdentifier(objectIdentifierStr), result, 5, WalkMode.WithinSubtree, null, null);
            return result;
        }

        private async Task SnmpSetAsync(List<Variable> variables)
            => await Messenger.SetAsync(VersionCode.V2, ipEndPoint, writeCommunity, variables);

        public async Task<Dictionary<int, SnmpVlan>> ReadSnmpVlansAsync()
        {
            Dictionary<int, SnmpVlan> snmpVlans = new();
            List<Variable> portVlanStaticTable = await SnmpBulkWalkAsync(OID_DOT1Q_VLAN_STATIC_TABLE);
            foreach (Variable portVlanStaticTableRow in portVlanStaticTable)
            {
                SnmpVariableHelpers.IdParts idParts = portVlanStaticTableRow.GetIdParts();
                SnmpVlan snmpVlan = snmpVlans.GetAnyway(idParts.RowId, id => new SnmpVlan(id));
                switch (idParts.NodeId)
                {
                    case OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS:
                        snmpVlan.EgressPorts = portVlanStaticTableRow.Data.ToBytes().Skip(3).ToArray();
                        break;
                    case OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS:
                        snmpVlan.UntaggedPorts = portVlanStaticTableRow.Data.ToBytes().Skip(3).ToArray();
                        break;
                }
            }
            return snmpVlans;
        }

        private const string OID_DOT1Q_VLAN_STATIC_TABLE = "1.3.6.1.2.1.17.7.1.4.3";
        private const string OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS = "1.3.6.1.2.1.17.7.1.4.3.1.2";
        private const string OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS = "1.3.6.1.2.1.17.7.1.4.3.1.4";

        public void BindUserToSnmpVlans(Dictionary<int, SnmpVlan> snmpVlans)
        {
            foreach (UserVlan userVlan in config.Vlans.Values)
                if (snmpVlans.TryGetValue(userVlan.ID, out SnmpVlan snmpVlan))
                    snmpVlan.UserVlan = userVlan;
        }

        public async Task<Dictionary<int, SnmpPort>> ReadSnmpPortsAsync()
        {
            Dictionary<int, SnmpPort> snmpPorts = new();
            List<Variable> portVlanTable = await SnmpBulkWalkAsync(OID_DOT1Q_PORT_VLAN_TABLE);
            foreach (Variable portVlanTableRow in portVlanTable)
            {
                SnmpVariableHelpers.IdParts idParts = portVlanTableRow.GetIdParts();
                SnmpPort snmpPort = snmpPorts.GetAnyway(idParts.RowId, id => new SnmpPort(id));
                switch (idParts.NodeId)
                {
                    case OID_DOT1Q_PVID:
                        if (int.TryParse(portVlanTableRow.Data.ToString(), out int pvid))
                            snmpPort.PVID = pvid;
                        break;
                }
            }
            return snmpPorts;
        }

        private const string OID_DOT1Q_PORT_VLAN_TABLE = "1.3.6.1.2.1.17.7.1.4.5";
        private const string OID_DOT1Q_PVID = "1.3.6.1.2.1.17.7.1.4.5.1.1";

        public void CalculateSnmpPortVlanMemberships(Dictionary<int, SnmpVlan> snmpVlans, Dictionary<int, SnmpPort> snmpPorts)
        {
            foreach (UserPort userPort in ports)
            {
                if (!snmpPorts.TryGetValue(userPort.Index, out SnmpPort snmpPort))
                {
                    userPort.CurrentVlan = null;
                    continue;
                }
                (int portByteIndex, int portBitIndex) = getByteBitIndex(userPort.Index);
                int ownerVlans = 0;
                SnmpVlan lastOwnerSnmpVlan = null;
                foreach (SnmpVlan snmpVlan in snmpVlans.Values)
                {
                    bool isUntagged = snmpVlan.UntaggedPorts.GetBit(portByteIndex, portBitIndex);
                    bool isEgress = snmpVlan.EgressPorts.GetBit(portByteIndex, portBitIndex);
                    if (isUntagged && isEgress)
                    {
                        ownerVlans++;
                        lastOwnerSnmpVlan = snmpVlan;
                    }
                }
                if ((ownerVlans == 1) && (lastOwnerSnmpVlan.ID == snmpPort.PVID) && userPort.Vlans.Contains(lastOwnerSnmpVlan.UserVlan))
                    userPort.CurrentVlan = lastOwnerSnmpVlan.UserVlan;
                else
                    userPort.CurrentVlan = null;
            }
        }

        private (int, int) getByteBitIndex(int portIndex) => (((portIndex - 1) / 8), (7 - ((portIndex - 1) % 8)));

        public async Task SetPortToVlanAsync(UserPort port, UserVlan vlan)
        {
            List<Variable> variablesFirst = new(), variablesLast = new();
            variablesFirst.Add(new Variable(new ObjectIdentifier($"{OID_DOT1Q_PVID}.{port.Index}"), new Gauge32(vlan.ID)));
            (int portByteIndex, int portBitIndex) = getByteBitIndex(port.Index);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_EGRESS_PORTS, vlan.ID, portByteIndex, portBitIndex, variablesFirst, variablesLast);
            await getVlansBitfieldsForPort(OID_DOT1Q_VLAN_STATIC_UNTAGGED_PORTS, vlan.ID, portByteIndex, portBitIndex, variablesFirst, variablesLast);
            variablesFirst.AddRange(variablesLast);
            await SnmpSetAsync(variablesFirst);
            portUpdated(port);   
        }

        private async Task getVlansBitfieldsForPort(string tableObjectIdentifier, int targetVlanId, int portByteIndex, int portBitIndex, List<Variable> variablesFirst, List<Variable> variablesLast)
        {
            foreach (Variable oldRow in await SnmpBulkWalkAsync(tableObjectIdentifier))
            {
                SnmpVariableHelpers.IdParts idParts = oldRow.GetIdParts();
                bool valueToSet = idParts.RowId == targetVlanId;
                byte[] snmpDataBytes = oldRow.Data.ToBytes();
                snmpDataBytes.SetBit(portByteIndex + 3, portBitIndex, valueToSet);
                Variable newRow = new Variable(oldRow.Id, DataFactory.CreateSnmpData(snmpDataBytes));
                (valueToSet ? variablesLast : variablesFirst).Add(newRow);
            }
        }

        private void portUpdated(UserPort port)
        {
            if (portsWithPendingChange.Contains(port))
                return;
            portsWithPendingChange.Add(port);
            PortsWithPendingChangeCount++;
        }

        private void changesPersisted()
        {
            portsWithPendingChange.Clear();
            PortsWithPendingChangeCount = 0;
        }

        public async Task<bool> PersistChangesAsync()
        {
            foreach (Func<Task> persistChangesDelegate in persistChangesDelegates)
            {
                try
                {
                    await persistChangesDelegate.Invoke();
                    changesPersisted();
                    return true;
                }
                catch { }
            }
            return false;
        }

        private Func<Task>[] persistChangesDelegates
        {
            get
            {
                if (_persistChangesDelegates == null)
                {
                    _persistChangesDelegates = new Func<Task>[]
                    {
                        persistChangesGeneralAsync,
                        persistChangesCiscoCopyAsync
                    };
                }
                return _persistChangesDelegates;
            }
        }

        private Func<Task>[] _persistChangesDelegates;

        private async Task persistChangesGeneralAsync()
        {
            await SnmpSetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier(OID_WRITEMEM), new Integer32(1))
            });
        }

        private const string OID_WRITEMEM = "1.3.6.1.4.1.9.2.1.54";

        private async Task persistChangesCiscoCopyAsync()
        {
            int randomRowId = _randomGenerator.Next(1, 512);
            await SnmpSetAsync(new List<Variable>() {
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_SOURCE_FILE_TYPE}.{randomRowId}"), new Integer32(4)),
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_DESTINATION_FILE_TYPE}.{randomRowId}"), new Integer32(3)),
                new Variable(new ObjectIdentifier($"{OID_CC_COPY_ENTRY_ROW_STATUS}.{randomRowId}"), new Integer32(1))
            });
        }

        private const string OID_CC_COPY_SOURCE_FILE_TYPE = "1.3.6.1.4.1.9.9.96.1.1.1.1.3";
        private const string OID_CC_COPY_DESTINATION_FILE_TYPE = "1.3.6.1.4.1.9.9.96.1.1.1.1.4";
        private const string OID_CC_COPY_ENTRY_ROW_STATUS = "1.3.6.1.4.1.9.9.96.1.1.1.1.14";

        private Random _randomGenerator = new Random();
    }

}
