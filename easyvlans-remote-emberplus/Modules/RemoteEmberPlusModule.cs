using easyvlans.Model.Deserializers;
namespace easyvlans.Modules
{
    class RemoteEmberPlusModule : IModule
    {
        public void Init()
        {
            RemoteMethodsDeserializer.Instance.Register(new MyEmberPlusAgensDeserializer());
        }
    }
}
