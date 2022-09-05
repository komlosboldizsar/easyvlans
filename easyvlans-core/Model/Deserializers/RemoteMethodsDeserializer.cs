using B.XmlDeserializer;
using easyvlans.Model.Remote;

namespace easyvlans.Model.Deserializers
{
    public sealed class RemoteMethodsDeserializer : HeterogenousListDeserializer<IRemoteMethod, Config>
    {
        public static RemoteMethodsDeserializer Instance { get; } = new();
        private RemoteMethodsDeserializer()
            : base(ConfigTagNames.REMOTES) { }
    }
}
