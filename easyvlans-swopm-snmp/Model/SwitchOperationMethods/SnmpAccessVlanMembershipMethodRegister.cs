namespace easyvlans.Model.SwitchOperationMethods
{
    public sealed class SnmpAccessVlanMembershipMethodRegister : MethodRegisterBase<ISnmpAccessVlanMembershipMethod, ISnmpAccessVlanMembershipMethod.IFactory>
    {
        public static SnmpAccessVlanMembershipMethodRegister Instance { get; } = new();
        private SnmpAccessVlanMembershipMethodRegister() { }
        public ISnmpAccessVlanMembershipMethod GetMethodInstance(string code, string @params, ISnmpSwitchOperationMethodCollection parent) => getFactory(code)?.GetInstance(@params, parent);
    }
}