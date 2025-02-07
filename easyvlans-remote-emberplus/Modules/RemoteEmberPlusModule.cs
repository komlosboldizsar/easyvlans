using easyvlans.Model.Deserializers;
using easyvlans.Model.Remote.EmberPlus;
namespace easyvlans.Modules
{
    class RemoteEmberPlusModule : IModule
    {
        public void Init()
        {
            RemoteMethodsDeserializer.Instance.Register(new MyEmberPlusProviderDeserializer());
        }
    }
}
