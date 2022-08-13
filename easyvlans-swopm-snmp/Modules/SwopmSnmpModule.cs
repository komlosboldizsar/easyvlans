using easyvlans.Model.Deserializers;
using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpModule : IModule
    {
        public void Init()
        {
            SwitchOperationMethodsDeserializer.Instance.Register(new SnmpV1SwitchOperationMethodCollection.Deserializer());
            SwitchOperationMethodsDeserializer.Instance.Register(new SnmpV2SwitchOperationMethodCollection.Deserializer());
        }
    }
}
