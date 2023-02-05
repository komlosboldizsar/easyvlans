using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpAccessVlanMembershipDlinkDgs121024axMethod : SnmpAccessVlanMembershipDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121024ax";

        public class Factory : ISnmpAccessVlanMembershipMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpAccessVlanMembershipMethod GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpAccessVlanMembershipDlinkDgs121024axMethod(data, parent);
        }

        public SnmpAccessVlanMembershipDlinkDgs121024axMethod(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
            : base(data, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 10;

    }

}
