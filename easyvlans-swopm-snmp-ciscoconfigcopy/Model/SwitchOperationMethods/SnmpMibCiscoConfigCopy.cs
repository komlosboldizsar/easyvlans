namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibCiscoConfigCopy : ISnmpMib
    {

        public const string MIB_NAME = "ciscoconfigcopy";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override IPersistChangesMethod createPersistChangesMethod(ISnmpConnection snmpConnection, object commonData)
                => new PersistChangesMethod(snmpConnection);

        }

        private const string OID_CC_COPY_ENTRY = "1.3.6.1.4.1.9.9.96.1.1.1.1";
        private const int COLUMN_SOURCE_FILE_TYPE = 3;
        private const int COLUMN_DEST_FILE_TYPE = 4;
        private const int COLUMN_ENTRY_ROW_STATUS = 14;
        private const int TXCONV_CONFIGFILETYPE_STARTUP = 3;
        private const int TXCONV_CONFIGFILETYPE_RUNNING = 4;
        private const int TXCONV_ROWSTATUS_ACTIVE = 1;

    }

}
