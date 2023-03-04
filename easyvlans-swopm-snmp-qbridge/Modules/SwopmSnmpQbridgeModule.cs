using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpQbridgeModule : IModule
    {
        public void Init()
            => SnmpSwitchOperationMethodCollectionDeserializerBase.RegisterMibDeserializer(new SnmpMibQBridge.Deserializer());
    }
}
