namespace easyvlans.Model
{
    internal sealed class SnmpAccessVlanMembershipMethodRegister : MethodRegisterBase<ISnmpAccessVlanMembershipMethod, ISnmpAccessVlanMembershipMethod.IFactory>
    {
        public static SnmpAccessVlanMembershipMethodRegister Instance { get; } = new();
        private SnmpAccessVlanMembershipMethodRegister() { }
        public ISnmpAccessVlanMembershipMethod GetMethodInstance(string code, ISnmpSwitchOperationMethodCollection parent) => getFactory(code)?.GetInstance(parent);
        protected override ISnmpAccessVlanMembershipMethod.IFactory[] KnownFactories { get; } = new ISnmpAccessVlanMembershipMethod.IFactory[]
        {
            new SnmpAccessVlanMembershipQBridgeMibMethod.Factory(),
            new SnmpAccessVlanMembershipDlinkDgs121024axMethod.Factory()
        };
    }
}