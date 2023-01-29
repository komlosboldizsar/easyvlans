using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpCiscosbcopyModule : IModule
    {
        public void Init()
        {
            SnmpPersistChangesMethodRegister.Instance.RegisterFactory(new SnmpPersistChangesCiscoSbCopyMethod.Factory());
        }
    }
}
