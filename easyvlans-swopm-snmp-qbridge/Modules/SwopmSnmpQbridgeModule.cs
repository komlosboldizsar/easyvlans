using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpQbridgeModule : IModule
    {
        public void Init()
        {
            SnmpAccessVlanMembershipMethodRegister.Instance.RegisterFactory(new SnmpAccessVlanMembershipQBridgeMethod.Factory());
        }
    }
}
