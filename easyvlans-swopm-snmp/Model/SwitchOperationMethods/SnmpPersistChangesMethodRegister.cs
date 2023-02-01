namespace easyvlans.Model.SwitchOperationMethods
{
    public sealed class SnmpPersistChangesMethodRegister : MethodRegisterBase<ISnmpPersistChangesMethod, ISnmpPersistChangesMethod.IFactory>
    {
        public static SnmpPersistChangesMethodRegister Instance { get; } = new();
        private SnmpPersistChangesMethodRegister() { }
        public ISnmpPersistChangesMethod GetMethodInstance(string code, string @params, ISnmpSwitchOperationMethodCollection parent) => getFactory(code)?.GetInstance(@params, parent);
        protected override string RegisterName => "SNMP persist changes";
    }
}
