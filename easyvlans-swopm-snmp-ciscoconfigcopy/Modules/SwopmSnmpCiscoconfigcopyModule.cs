using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpCiscoconfigcopyModule : IModule
    {
        public void Init()
        {
            SnmpPersistChangesMethodRegister.Instance.RegisterFactory(new SnmpPersistChangesCiscoConfigCopyMethod.Factory());
        }
    }
}
