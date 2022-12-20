namespace easyvlans.Model.SwitchOperationMethods
{

    internal sealed class SnmpPersistChangesDlinkDgs121024axMethod : SnmpPersistChangesDlinkDgs1210MethodBase
    {

        public const string CODE = "dlinkdgs121024ax";

        public class Factory : ISnmpPersistChangesMethod.IFactory
        {
            public string Code => CODE;
            public ISnmpPersistChangesMethod GetInstance(string @params, ISnmpSwitchOperationMethodCollection parent)
                => new SnmpPersistChangesDlinkDgs121024axMethod(@params, parent);
        }

        public SnmpPersistChangesDlinkDgs121024axMethod(string @params, ISnmpSwitchOperationMethodCollection parent)
            : base(@params, parent) { }

        public override string Code => CODE;
        public override int MibSubtreeIndex => 10;

    }

}
