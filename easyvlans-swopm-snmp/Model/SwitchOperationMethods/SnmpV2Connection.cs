using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal class SnmpV2Connection : SnmpV1V2Connection
    {

        public SnmpV2Connection(string ip, int port, string communityStrings)
            : base(ip, port, communityStrings) { }

        public override async Task<List<Variable>> WalkAsync(string objectIdentifierStr)
        {
            List<Variable> result = new();
            await Messenger.BulkWalkAsync(VersionCode.V2, _ipEndPoint, _readCommunityString, OctetString.Empty,
                new ObjectIdentifier(objectIdentifierStr), result, 5, WalkMode.WithinSubtree, null, null);
            return result;
        }

        public override async Task SetAsync(List<Variable> variables)
            => await Messenger.SetAsync(VersionCode.V2, _ipEndPoint, _writeCommunityString, variables);

    }
}
