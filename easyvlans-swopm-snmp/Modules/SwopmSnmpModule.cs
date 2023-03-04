using easyvlans.Model.Deserializers;
using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpModule : IModule
    {
        public void Init()
        {
            SwitchDeserializer.RegisterOperationMethodsDeserializer(new SnmpV1SwitchOperationMethodCollectionDeserializer());
            SwitchDeserializer.RegisterOperationMethodsDeserializer(new SnmpV2SwitchOperationMethodCollectionDeserializer());
        }
    }
}
