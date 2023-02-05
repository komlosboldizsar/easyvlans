using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    public sealed class SnmpPersistChangesMethodRegister : MethodRegisterBase<ISnmpPersistChangesMethod, ISnmpPersistChangesMethod.IFactory>
    {
        public static SnmpPersistChangesMethodRegister Instance { get; } = new();
        private SnmpPersistChangesMethodRegister() { }
        public ISnmpPersistChangesMethod GetMethodInstance(string code, XmlNode data, ISnmpSwitchOperationMethodCollection parent) => getFactory(code)?.GetInstance(data, parent);
        protected override string RegisterName => "SNMP persist changes";
    }
}
