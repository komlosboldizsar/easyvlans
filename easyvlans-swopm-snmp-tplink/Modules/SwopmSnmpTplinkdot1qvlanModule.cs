using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpTplinkdot1qvlanModule : IModule
    {
        public void Init()
        {
            SnmpAccessVlanMembershipMethodRegister.Instance.RegisterFactory(new SnmpAccessVlanMembershipTPLinkDot1qVlanMethod.Factory());
            SnmpPersistChangesMethodRegister.Instance.RegisterFactory(new SnmpPersistChangesTPLinkSysToolMethod.Factory());
        }
    }
}
