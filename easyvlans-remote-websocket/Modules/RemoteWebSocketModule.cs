using easyvlans.Model.Deserializers;
using easyvlans.Model.Remote.WebSocket;
namespace easyvlans.Modules
{
    class RemoteWebSocketModule : IModule
    {
        public void Init()
        {
            RemoteMethodsDeserializer.Instance.Register(new MyWebSocketServerDeserializer());
        }
    }
}
