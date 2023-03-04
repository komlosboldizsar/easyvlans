using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibCiscoConfigCopy
    {
        internal class PersistChangesMethod : SnmpMethodBase, IPersistChangesMethod
        {

            public PersistChangesMethod(ISnmpConnection snmpConnection) : base(snmpConnection) { }

            public override string MibName => MIB_NAME;

            public async Task DoAsync()
            {
                int randomRowId = randomGenerator.Next(1, 512);
                await _snmpConnection.SetAsync(new List<Variable>() {
                    new Variable(new ObjectIdentifier($"{OID_CC_COPY_ENTRY}.{COLUMN_SOURCE_FILE_TYPE}.{randomRowId}"), new Integer32(TXCONV_CONFIGFILETYPE_RUNNING)),
                    new Variable(new ObjectIdentifier($"{OID_CC_COPY_ENTRY}.{COLUMN_DEST_FILE_TYPE}.{randomRowId}"), new Integer32(TXCONV_CONFIGFILETYPE_STARTUP)),
                    new Variable(new ObjectIdentifier($"{OID_CC_COPY_ENTRY}.{COLUMN_ENTRY_ROW_STATUS}.{randomRowId}"), new Integer32(TXCONV_ROWSTATUS_ACTIVE))
                });
            }

            private static readonly Random randomGenerator = new();

        }
    }
}
