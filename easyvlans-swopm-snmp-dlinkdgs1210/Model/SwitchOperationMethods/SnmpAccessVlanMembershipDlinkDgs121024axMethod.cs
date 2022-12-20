namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121024axMethod : SnmpAccessVlanMembershipDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121024ax";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(string @params, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121024axMethod(@params, parent);
        }

        public SnmpAccessVlanMembershipDlinkDgs121024axMethod(string @params, ISnmpSwitchOperationMethodCollection parent)
            : base(@params, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 10;

    }

}
