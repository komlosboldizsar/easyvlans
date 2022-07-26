using System.Xml;

namespace easyvlans.Model.SwitchOperationMethods
{
    public sealed class SwitchOperationMethodCollectionRegister : MethodRegisterBase<ISwitchOperationMethodCollection, ISwitchOperationMethodCollection.IFactory>
    {
        public static SwitchOperationMethodCollectionRegister Instance { get; } = new();
        private SwitchOperationMethodCollectionRegister() { }
        public ISwitchOperationMethodCollection GetMethodInstance(XmlNode configNode, Switch @switch) => getFactory(configNode.LocalName)?.GetInstance(configNode, @switch);
    }
}
