namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121024axMethod : SnmpAccessVlanMembershipDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121024ax";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121024axMethod(parent);
        }

        public SnmpAccessVlanMembershipDlinkDgs121024axMethod(ISnmpSwitchOperationMethodCollection parent)
            : base(parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 10;

    }

}
