namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed partial class SnmpMibRfc1213 : ISnmpMib
    {

        public const string MIB_NAME = "rfc1213";

        public class Deserializer : SnmpMibDeserializerBase
        {

            public override string ElementName => MIB_NAME;

            protected override IReadSwitchUptimeMethod createReadSwitchUptimeMethod(ISnmpConnection snmpConnection, object commonData)
                => new ReadSwitchUptimeMethod(snmpConnection);

        }

        private const string OID_SYS_UP_TIME = "1.3.6.1.2.1.1.3.0";

    }

}
