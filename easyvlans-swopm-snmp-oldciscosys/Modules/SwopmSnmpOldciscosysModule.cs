using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpOldciscosysModule : IModule
    {
        public void Init()
        {
            SnmpPersistChangesMethodRegister.Instance.RegisterFactory(new SnmpPersistChangesOldCiscoSysMethod.Factory());
        }
    }
}
