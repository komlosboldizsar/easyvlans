using B.XmlDeserializer;
using easyvlans.Logger;
using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Model.Deserializers
{
    public sealed class SwitchOperationMethodsDeserializer : HeterogenousListDeserializer<ISwitchOperationMethodCollection, Config>
    {
        public static SwitchOperationMethodsDeserializer Instance { get; } = new();
        private SwitchOperationMethodsDeserializer()
            : base(ConfigTagNames.SWITCH) { }

        public void Register(ISwitchOperationMethodCollection.IDeserializer deserializer)
        {
            LogDispatcher.VV($"Registered XML deserializer for switch operation methods: [{deserializer.Code}]");
            base.Register(deserializer);
        }

    }
}
