using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpOldciscosysModule : IModule
    {
        public void Init()
            => SnmpSwitchOperationMethodCollectionDeserializerBase.RegisterMibDeserializer(new SnmpMibOldCiscoSys.Deserializer());
    }
}
