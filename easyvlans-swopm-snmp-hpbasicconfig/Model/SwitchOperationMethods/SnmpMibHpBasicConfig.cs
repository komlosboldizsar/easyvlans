namespace easyvlans.Model.SwitchOperationMethods
{
    internal sealed partial class SnmpMibHpBasicConfig : ISnmpMib
    {

        public const string MIB_NAME = "hpbasicconfig";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override IPersistChangesMethod createPersistChangesMethod(ISnmpConnection snmpConnection, object commonData)
                => new PersistChangesMethod(snmpConnection);

        }

        private const string OID_SAVECONFIG = "1.3.6.1.4.1.11.2.14.11.5.1.7.1.29.1.1";
        private const int TXCONV_SAVECONFIG_SAVECONFIG = 2;

    }
}
