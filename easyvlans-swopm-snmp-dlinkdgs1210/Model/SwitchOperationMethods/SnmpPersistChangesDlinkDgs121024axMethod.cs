namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesDlinkDgs121024axMethod : SnmpPersistChangesDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121024ax";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesDlinkDgs121024axMethod(parent);
        }

        public SnmpPersistChangesDlinkDgs121024axMethod(ISnmpSwitchOperationMethodCollection parent)
            : base(parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 10;

    }

}
