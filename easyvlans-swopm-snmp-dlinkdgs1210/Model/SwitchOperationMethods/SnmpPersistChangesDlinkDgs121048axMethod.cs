namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesDlinkDgs121048axMethod : SnmpPersistChangesDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121048ax";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(string @params, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesDlinkDgs121048axMethod(@params, parent);
        }

        public SnmpPersistChangesDlinkDgs121048axMethod(string @params, ISnmpSwitchOperationMethodCollection parent)
            : base(@params, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 11;

    }

}
