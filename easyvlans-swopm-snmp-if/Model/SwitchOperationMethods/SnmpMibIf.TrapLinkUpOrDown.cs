using BToolbox.Helpers;
using easyvlans.Helpers;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibIf
    {
        internal class TrapLinkUpOrDown : SnmpTrapSubscriberBase
        {

            public TrapLinkUpOrDown(ISnmpConnection snmpConnection, CommonData commonData)
                : base(snmpConnection)
            {
                _commonData = commonData;
            }

            private readonly CommonData _commonData;

            public override string MibName => MIB_NAME;

            public override void TrapReceived(ISnmpMessage message, IEnumerable<Variable> variables)
            {
                Variable variableIfIndex = variables.Where(v => v.GetIdParts().NodeId == OID_IF_INDEX).FirstOrDefault();
                if (variableIfIndex == null)
                    return;
                int ifIndex = -1;
                if (!variableIfIndex.ToInt(i => ifIndex = i) || (ifIndex == -1))
                    return;
                Port userPort = _snmpConnection.Switch.GetPort(ifIndex - _commonData.PortIndexOffset);
                if (userPort != null)
                    _ = Task.Run(() => userPort.Switch.ReadInterfaceStatusAsync(userPort));
            }

        }
    }
}
