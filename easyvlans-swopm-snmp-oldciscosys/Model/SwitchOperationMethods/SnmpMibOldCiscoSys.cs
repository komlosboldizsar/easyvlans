namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibOldCiscoSys : ISnmpMib
    {

        public const string MIB_NAME = "oldciscosys";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override IPersistChangesMethod createPersistChangesMethod(ISnmpConnection snmpConnection, object commonData)
                => new PersistChangesMethod(snmpConnection);

        }

        private const string OID_WRITEMEM = "1.3.6.1.4.1.9.2.1.54";
        private const int TXCONV_WRITEMEM_WRITE = 1;

    }

}
