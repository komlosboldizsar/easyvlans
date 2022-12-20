namespace easyvlans.Model.SwitchOperationMethods
{
    internal class SnmpV2SwitchOperationMethodCollection : SnmpV1V2SwitchOperationMethodCollectionBase
    {

        private const string CODE = "snmpv2";

        public class Deserializer : FactoryBase
        {
            public override string Code => CODE;
            protected override ISwitchOperationMethodCollection createInstance(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, string accessVlanMembershipMethodParams, string persistChangesMethodName, string persistChangesMethodParams)
                => new SnmpV2SwitchOperationMethodCollection(@switch, ip, port, communityStrings, accessVlanMembershipMethodName, accessVlanMembershipMethodParams, persistChangesMethodName, persistChangesMethodParams);
        }

        public SnmpV2SwitchOperationMethodCollection(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, string accessVlanMembershipMethodParams, string persistChangesMethodName, string persistChangesMethodParams)
            : base(@switch, accessVlanMembershipMethodName, accessVlanMembershipMethodParams, persistChangesMethodName, persistChangesMethodParams)
        {
            SnmpConnection = new SnmpV2Connection(ip, port, communityStrings);
        }

        public override string Code => CODE;

    }
}
