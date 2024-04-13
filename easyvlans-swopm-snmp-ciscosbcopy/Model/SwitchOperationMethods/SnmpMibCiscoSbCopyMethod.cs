namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibCiscoSbCopyMethod : ISnmpMib
    {

        public const string MIB_NAME = "ciscosbcopy";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override IPersistChangesMethod createPersistChangesMethod(ISnmpConnection snmpConnection, object commonData)
                => new PersistChangesMethod(snmpConnection);

        }

        private const string OID_RL_COPY_ENTRY = "1.3.6.1.4.1.9.6.1.101.87.2.1";
        private const int COLUMN_SOURCE_FILE_TYPE = 7;
        private const int COLUMN_DESTINATION_FILE_TYPE = 12;
        private const int COLUMN_SOURCE_LOCATION = 3;
        private const int COLUMN_DESTIONATION_LOCATION = 8;
        private const int COLUMN_ENTRY_ROW_STATUS = 17;

    }

}
