using BToolbox.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibIf : ISnmpMib
    {

        public const string MIB_NAME = "if";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override IReadInterfaceStatusMethod createReadInterfaceStatusMethod(ISnmpConnection snmpConnection, object commonData)
                => new ReadInterfaceStatusMethod(snmpConnection);

        }

        private const string OID_IF_TABLE = "1.3.6.1.2.1.2.2.1";
        private const string OID_IF_ADMIN_STATUS = $"{OID_IF_TABLE}.7";
        private const string OID_IF_OPER_STATUS = $"{OID_IF_TABLE}.8";
        private const string OID_IF_LAST_CHANGE = $"{OID_IF_TABLE}.9";

    }

}
