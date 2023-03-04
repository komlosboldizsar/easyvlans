using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpHpbasicconfigModule : IModule
    {
        public void Init()
            => SnmpSwitchOperationMethodCollectionDeserializerBase.RegisterMibDeserializer(new SnmpMibHpBasicConfig.Deserializer());
    }
}
