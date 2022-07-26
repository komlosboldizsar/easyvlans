using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpModule : IModule
    {
        public void Init()
        {
            SwitchOperationMethodCollectionRegister.Instance.RegisterFactory(new SnmpV1SwitchOperationMethodCollection.Factory());
            SwitchOperationMethodCollectionRegister.Instance.RegisterFactory(new SnmpV2SwitchOperationMethodCollection.Factory());
        }
    }
}
