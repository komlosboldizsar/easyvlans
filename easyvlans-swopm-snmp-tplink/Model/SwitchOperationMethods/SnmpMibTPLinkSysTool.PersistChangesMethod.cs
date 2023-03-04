using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibTPLinkSysTool
    {
        internal class PersistChangesMethod : SnmpMethodBase, IPersistChangesMethod
        {

            public PersistChangesMethod(ISnmpConnection snmpConnection) : base(snmpConnection) { }

            public override string MibName => MIB_NAME;

            public async Task DoAsync()
                => await _snmpConnection.SetAsync(new List<Variable>() {
                    new Variable(new ObjectIdentifier($"{OID_TSYSTOOLCONFIGSAVE}"), new Integer32(TXCONV_TSYSTOOLCONFIGSAVE_COMMIT))
                });

        }
    }
}
