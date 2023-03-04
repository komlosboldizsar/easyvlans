using Lextm.SharpSnmpLib;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibCiscoSbCopyMethod
    {
        internal class PersistChangesMethod : SnmpMethodBase, IPersistChangesMethod
        {

            public PersistChangesMethod(ISnmpConnection snmpConnection) : base(snmpConnection) { }

            public override string MibName => MIB_NAME;

            public async Task DoAsync()
            {
                int randomRowId = randomGenerator.Next(1, 512);
                await _snmpConnection.SetAsync(new List<Variable>() {
                    new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_SOURCE_FILE_TYPE}.{randomRowId}"), new Integer32(2)),
                    new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_DESTINATION_FILE_TYPE}.{randomRowId}"), new Integer32(3)),
                    new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_SOURCE_LOCATION}.{randomRowId}"), new Integer32(1)),
                    new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_DESTIONATION_LOCATION}.{randomRowId}"), new Integer32(1)),
                    new Variable(new ObjectIdentifier($"{OID_RL_COPY_ENTRY}.{COLUMN_ENTRY_ROW_STATUS}.{randomRowId}"), new Integer32(4)),
                });
            }

            private static readonly Random randomGenerator = new();

        }
    }
}
