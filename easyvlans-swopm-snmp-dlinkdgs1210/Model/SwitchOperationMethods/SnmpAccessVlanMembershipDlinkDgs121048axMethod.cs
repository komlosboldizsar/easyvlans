using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121048axMethod : SnmpAccessVlanMembershipDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121048ax";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121048axMethod(data, parent);
        }

        public SnmpAccessVlanMembershipDlinkDgs121048axMethod(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
            : base(data, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 11;

    }

}
