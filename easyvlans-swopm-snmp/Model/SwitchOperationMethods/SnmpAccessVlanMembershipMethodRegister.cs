using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    public sealed class SnmpAccessVlanMembershipMethodRegister : MethodRegisterBase<ISnmpAccessVlanMembershipMethod, ISnmpAccessVlanMembershipMethod.IFactory>
    {
        public static SnmpAccessVlanMembershipMethodRegister Instance { get; } = new();
        private SnmpAccessVlanMembershipMethodRegister() { }
        public ISnmpAccessVlanMembershipMethod GetMethodInstance(string code, XmlNode data, ISnmpSwitchOperationMethodCollection parent) => getFactory(code)?.GetInstance(data, parent);
        protected override string RegisterName => "SNMP access vlan membership";
    }
}