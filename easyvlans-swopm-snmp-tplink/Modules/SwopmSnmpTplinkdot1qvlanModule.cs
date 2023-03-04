using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Modules
{
    public class SwopmSnmpTplinkdot1qvlanModule : IModule
    {
        public void Init()
        {
            SnmpSwitchOperationMethodCollectionDeserializerBase.RegisterMibDeserializer(new SnmpMibTPLinkDot1qVlan.Deserializer());
            SnmpSwitchOperationMethodCollectionDeserializerBase.RegisterMibDeserializer(new SnmpMibTPLinkSysTool.Deserializer());
        }
    }
}
