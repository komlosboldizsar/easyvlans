using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpQbridgemibModule : IModule
    {
        public void Init()
        {
            SnmpAccessVlanMembershipMethodRegister.Instance.RegisterFactory(new SnmpAccessVlanMembershipQBridgeMibMethod.Factory());
        }
    }
}
