using B.XmlDeserializer.Context;
using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    public sealed class SnmpAccessVlanMembershipMethodRegister : MethodRegisterBase<ISnmpAccessVlanMembershipMethod, ISnmpAccessVlanMembershipMethod.IFactory>
    {
        public static SnmpAccessVlanMembershipMethodRegister Instance { get; } = new();
        private SnmpAccessVlanMembershipMethodRegister() { }
        public ISnmpAccessVlanMembershipMethod GetMethodInstance(string code, XmlNode data, DeserializationContext deserializationContext, ISnmpSwitchOperationMethodCollection parent) => getFactory(code)?.GetInstance(data, deserializationContext, parent);
        protected override string RegisterName => "SNMP access vlan membership";
    }
}