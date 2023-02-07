using B.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    public sealed class SnmpPersistChangesMethodRegister : MethodRegisterBase<ISnmpPersistChangesMethod, ISnmpPersistChangesMethod.IFactory>
    {
        public static SnmpPersistChangesMethodRegister Instance { get; } = new();
        private SnmpPersistChangesMethodRegister() { }
        public ISnmpPersistChangesMethod GetMethodInstance(string code, XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent) => getFactory(code)?.GetInstance(data, deserializationContext, parent);
        protected override string RegisterName => "SNMP persist changes";
    }
}
