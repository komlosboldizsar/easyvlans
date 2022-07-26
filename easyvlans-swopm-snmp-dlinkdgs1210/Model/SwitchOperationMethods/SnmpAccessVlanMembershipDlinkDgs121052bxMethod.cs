namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121052bxMethod : SnmpAccessVlanMembershipDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121052bx";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121052bxMethod(parent);
        }

        public SnmpAccessVlanMembershipDlinkDgs121052bxMethod(ISnmpSwitchOperationMethodCollection parent)
            : base(parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 17;

    }

}
