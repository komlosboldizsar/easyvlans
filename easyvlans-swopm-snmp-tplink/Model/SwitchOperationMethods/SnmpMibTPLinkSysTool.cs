using B.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibTPLinkSysTool : ISnmpMib
    {

        public const string MIB_NAME = "tplinksystool";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override IPersistChangesMethod createPersistChangesMethod(ISnmpConnection snmpConnection, object commonData)
                => new PersistChangesMethod(snmpConnection);

        }

        private const string OID_TSYSTOOLCONFIGSAVE = "1.3.6.1.4.1.11863.6.3.1.3";
        private const int TXCONV_TSYSTOOLCONFIGSAVE_COMMIT = 1;

    }

}
