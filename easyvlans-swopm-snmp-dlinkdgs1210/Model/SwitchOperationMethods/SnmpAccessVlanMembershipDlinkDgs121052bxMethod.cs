namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121052bxMethod : SnmpAccessVlanMembershipDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121052bx";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(string @params, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121052bxMethod(@params, parent);
        }

        public SnmpAccessVlanMembershipDlinkDgs121052bxMethod(string @params, ISnmpSwitchOperationMethodCollection parent)
            : base(@params, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 17;

    }

}
