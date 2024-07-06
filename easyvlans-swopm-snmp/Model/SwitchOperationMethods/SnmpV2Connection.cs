using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal class SnmpV2Connection : SnmpV1V2Connection
    {

        public override string Version => "v2";
        public override VersionCode VersionCode => VersionCode.V2;

        public SnmpV2Connection(Switch @switch, string ip, int port, string communityStrings, int? trapPort, string trapCommunityString, bool trapVersionStrict)
            : base(@switch, ip, port, communityStrings, trapPort, trapCommunityString, trapVersionStrict) { }

        protected override async Task<List<Variable>> DoWalkAsync(string objectIdentifierStr)
        {
            List<Variable> result = new();
            await Messenger.BulkWalkAsync(VersionCode.V2, _ipEndPoint, _readCommunityString, OctetString.Empty,
                new ObjectIdentifier(objectIdentifierStr), result, 5, WalkMode.WithinSubtree, null, null);
            return result;
        }

        protected override async Task DoSetAsync(List<Variable> variables)
            => await Messenger.SetAsync(VersionCode.V2, _ipEndPoint, _writeCommunityString, variables);

    }
}
