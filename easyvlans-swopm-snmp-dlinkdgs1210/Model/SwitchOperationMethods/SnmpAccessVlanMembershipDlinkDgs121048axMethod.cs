namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121048axMethod : SnmpAccessVlanMembershipDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121048ax";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121048axMethod(parent);
        }

        public SnmpAccessVlanMembershipDlinkDgs121048axMethod(ISnmpSwitchOperationMethodCollection parent)
            : base(parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 11;

    }

}
