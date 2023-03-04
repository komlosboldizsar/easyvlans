using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal class SnmpV1Connection : SnmpV1V2Connection
    {

        public override string Version => "v1";

        public SnmpV1Connection(Switch @switch, string ip, int port, string communityStrings)
            : base(@switch, ip, port, communityStrings)
        {
            Messenger.UseFullRange = false;
        }

        protected override async Task<List<Variable>> DoWalkAsync(string objectIdentifierStr)
        {
            List<Variable> result = new();
            await Messenger.WalkAsync(VersionCode.V1, _ipEndPoint, _readCommunityString,
                new ObjectIdentifier(objectIdentifierStr), result, WalkMode.WithinSubtree);
            return result;
        }

        protected override async Task DoSetAsync(List<Variable> variables)
            => await Messenger.SetAsync(VersionCode.V1, _ipEndPoint, _writeCommunityString, variables);

    }
}
