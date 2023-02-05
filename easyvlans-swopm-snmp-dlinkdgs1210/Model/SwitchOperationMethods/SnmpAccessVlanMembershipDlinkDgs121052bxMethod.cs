using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121052bxMethod : SnmpAccessVlanMembershipDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121052bx";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121052bxMethod(data, parent);
        }

        public SnmpAccessVlanMembershipDlinkDgs121052bxMethod(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
            : base(data, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 17;

    }

}
