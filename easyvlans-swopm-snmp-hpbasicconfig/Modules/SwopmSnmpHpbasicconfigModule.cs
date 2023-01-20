using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpHpbasicconfigModule : IModule
    {
        public void Init()
        {
            SnmpPersistChangesMethodRegister.Instance.RegisterFactory(new SnmpPersistChangesHpBasicConfigMethod.Factory());
        }
    }
}
