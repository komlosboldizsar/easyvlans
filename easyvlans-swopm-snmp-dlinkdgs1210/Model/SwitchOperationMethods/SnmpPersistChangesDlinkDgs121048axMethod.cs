namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesDlinkDgs121048axMethod : SnmpPersistChangesDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121048ax";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesDlinkDgs121048axMethod(parent);
        }

        public SnmpPersistChangesDlinkDgs121048axMethod(ISnmpSwitchOperationMethodCollection parent)
            : base(parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 11;

    }

}
