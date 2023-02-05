using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesDlinkDgs121052bxMethod : SnmpPersistChangesDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121052bx";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesDlinkDgs121052bxMethod(data, parent);
        }

        public SnmpPersistChangesDlinkDgs121052bxMethod(XmlNode data, ISnmpSwitchOperationMethodCollection parent)
            : base(data, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 17;

    }

}
