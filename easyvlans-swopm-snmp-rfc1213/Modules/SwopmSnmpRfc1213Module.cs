using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpRfc1213Module : IModule
    {
        public void Init()
            => SnmpSwitchOperationMethodCollectionDeserializerBase.RegisterMibDeserializer(new SnmpMibRfc1213.Deserializer());
    }
}
