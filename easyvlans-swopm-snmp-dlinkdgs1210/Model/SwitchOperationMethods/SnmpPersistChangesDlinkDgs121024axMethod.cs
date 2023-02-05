using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesDlinkDgs121024axMethod : SnmpPersistChangesDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121024ax";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesDlinkDgs121024axMethod(data, parent);
        }

        public SnmpPersistChangesDlinkDgs121024axMethod(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
            : base(data, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 10;

    }

}
