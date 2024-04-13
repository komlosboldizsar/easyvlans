using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibOldCiscoSys
    {
        internal class PersistChangesMethod : SnmpMethodBase, IPersistChangesMethod
        {

            public PersistChangesMethod(ISnmpConnection snmpConnection) : base(snmpConnection) { }

            public override string MibName => MIB_NAME;

            async Task IPersistChangesMethod.DoAsync()
                => await _snmpConnection.SetAsync(new List<Variable>() {
                    new Variable(new ObjectIdentifier(OID_WRITEMEM), new Integer32(TXCONV_WRITEMEM_WRITE))
                });

        }
    }
}
