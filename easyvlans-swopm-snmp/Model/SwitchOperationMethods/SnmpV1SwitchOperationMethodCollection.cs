using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal class SnmpV1SwitchOperationMethodCollection : SnmpV1V2SwitchOperationMethodCollectionBase
    {

        private const string CODE = "snmpv1";

        public class Deserializer : FactoryBase
        {
            public override string Code => CODE;
            protected override ISwitchOperationMethodCollection createInstance(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, XmlNode accessVlanMembershipMethodData, string persistChangesMethodName, XmlNode persistChangesMethodData)
                => new SnmpV1SwitchOperationMethodCollection(@switch, ip, port, communityStrings, accessVlanMembershipMethodName, accessVlanMembershipMethodData, persistChangesMethodName, persistChangesMethodData);
        }

        public SnmpV1SwitchOperationMethodCollection(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, XmlNode accessVlanMembershipMethodData, string persistChangesMethodName, XmlNode persistChangesMethodData)
            : base(@switch, accessVlanMembershipMethodName, accessVlanMembershipMethodData, persistChangesMethodName, persistChangesMethodData)
            => SnmpConnection = new SnmpV1Connection(ip, port, communityStrings);

        public override string Code => CODE;

    }
}
