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

            public TrapLinkUpOrDown(ISnmpConnection snmpConnection)
                : base(snmpConnection) { }

            public override string MibName => MIB_NAME;

            public override void TrapReceived(ISnmpMessage message, IEnumerable<Variable> variables)
            {
                int ifIndex =-1, adminStatus = 0, operStatus = 0;
                foreach (Variable variable in variables)
                {
                    SnmpVariableHelpers.IdParts idParts = variable.GetIdParts();
                    switch (idParts.NodeId)
                    {
                        case OID_IF_INDEX:
                            variable.ToInt(i => ifIndex = i);
                            break;
                        case OID_IF_ADMIN_STATUS:
                            variable.ToInt(i => adminStatus = i);
                            break;
                        case OID_IF_OPER_STATUS:
                            variable.ToInt(i => operStatus = i);
                            break;
                    }
                }
                if (ifIndex == -1)
                    return;
                Port userPort = _snmpConnection.Switch.GetPort(ifIndex);
                if (userPort != null)
                    UpdatePort(userPort, adminStatus, operStatus);
            }

        }
    }
}
