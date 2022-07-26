namespace easyvlans.Model.SwitchOperationMethods
{
    internal class SnmpV2SwitchOperationMethodCollection : SnmpV1V2SwitchOperationMethodCollectionBase
    {

        private const string CODE = "snmpv2";

        public class Factory : FactoryBase
        {
            public override string Code => CODE;
            protected override ISwitchOperationMethodCollection createInstance(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, string persistChangesMethodName)
                => new SnmpV2SwitchOperationMethodCollection(@switch, ip, port, communityStrings, accessVlanMembershipMethodName, persistChangesMethodName);
        }

        public SnmpV2SwitchOperationMethodCollection(Switch @switch, string ip, int port, string communityStrings, string accessVlanMembershipMethodName, string persistChangesMethodName)
            : base(@switch, accessVlanMembershipMethodName, persistChangesMethodName)
        {
            SnmpConnection = new SnmpV2Connection(ip, port, communityStrings);
        }

        public override string Code => CODE;

    }
}
