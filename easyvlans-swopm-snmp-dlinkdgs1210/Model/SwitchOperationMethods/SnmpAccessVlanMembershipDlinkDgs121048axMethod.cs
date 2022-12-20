namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121048axMethod : SnmpAccessVlanMembershipDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121048ax";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(string @params, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121048axMethod(@params, parent);
        }

        public SnmpAccessVlanMembershipDlinkDgs121048axMethod(string @params, ISnmpSwitchOperationMethodCollection parent)
            : base(@params, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 11;

    }

}
