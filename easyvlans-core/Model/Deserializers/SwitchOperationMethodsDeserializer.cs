using B.XmlDeserializer;
using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Model.Deserializers
{
    public sealed class SwitchOperationMethodsDeserializer : HeterogenousListDeserializer<ISwitchOperationMethodCollection, Config>
    {
        public static SwitchOperationMethodsDeserializer Instance { get; } = new();
        private SwitchOperationMethodsDeserializer()
            : base(ConfigTagNames.SWITCH) { }
    }
}
