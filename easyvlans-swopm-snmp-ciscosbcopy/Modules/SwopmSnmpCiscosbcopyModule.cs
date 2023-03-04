using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpCiscosbcopyModule : IModule
    {
        public void Init()
            => SnmpSwitchOperationMethodCollectionDeserializerBase.RegisterMibDeserializer(new SnmpMibCiscoSbCopyMethod.Deserializer());
    }
}
