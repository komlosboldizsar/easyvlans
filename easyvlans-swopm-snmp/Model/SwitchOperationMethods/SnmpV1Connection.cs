using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal class SnmpV1Connection : SnmpV1V2Connection
    {

        public SnmpV1Connection(string ip, int port, string communityStrings)
            : base(ip, port, communityStrings) { }

        public override async Task<List<Variable>> WalkAsync(string objectIdentifierStr)
        {
            List<Variable> result = new();
            await Messenger.WalkAsync(VersionCode.V1, _ipEndPoint, _readCommunityString,
                new ObjectIdentifier(objectIdentifierStr), result, WalkMode.WithinSubtree);
            return result;
        }

        public override async Task SetAsync(List<Variable> variables)
            => await Messenger.SetAsync(VersionCode.V1, _ipEndPoint, _writeCommunityString, variables);

    }
}
