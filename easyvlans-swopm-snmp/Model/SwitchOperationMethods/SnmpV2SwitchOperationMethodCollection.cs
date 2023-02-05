using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal class SnmpV2SwitchOperationMethodCollection : SnmpV1V2SwitchOperationMethodCollectionBase
    {

        private const string CODE = "snmpv2";

        public class Deserializer : FactoryBase
        {
            public override string Code => CODE;
            protected override ISwitchOperationMethodCollection createInstance(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, XmlNode accessVlanMembershipMethodData, string persistChangesMethodName, XmlNode persistChangesMethodData)
                => new SnmpV2SwitchOperationMethodCollection(@switch, ip, port, communityStrings, accessVlanMembershipMethodName, accessVlanMembershipMethodData, persistChangesMethodName, persistChangesMethodData);
        }

        public SnmpV2SwitchOperationMethodCollection(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, XmlNode accessVlanMembershipMethodData, string persistChangesMethodName, XmlNode persistChangesMethodData)
            : base(@switch, accessVlanMembershipMethodName, accessVlanMembershipMethodData, persistChangesMethodName, persistChangesMethodData)
            => SnmpConnection = new SnmpV2Connection(ip, port, communityStrings);

        public override string Code => CODE;

    }
}
