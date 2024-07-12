using BToolbox.Model;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal class SnmpV1Connection : SnmpV1V2Connection
    {

        public override string Version => "v1";
        public override VersionCode VersionCode => VersionCode.V1;

        public SnmpV1Connection(Switch @switch, string ip, int port, string communityStrings, int? trapPort, string trapCommunityString, bool trapVersionStrict)
            : base(@switch, ip, port, communityStrings, trapPort, trapCommunityString, trapVersionStrict)
        {
            Messenger.UseFullRange = false;
        }

        protected override async Task<IList<Variable>> DoGetAsync(IEnumerable<string> objectIdentifierStrs)
        {
            IList<Variable> variables = objectIdentifierStrs.Select(oid => new Variable(new ObjectIdentifier(oid))).ToList();
            return await Messenger.GetAsync(VersionCode.V2, _ipEndPoint, _readCommunityString, variables);
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

        public override void SubscribeForTrap(ITrapSubscriber subscriber, GenericCode v1GenericCode, int? v1SpecificCode, string v1EnterpriseFilter, ObjectIdentifier v2TrapOid)
            => _trapReceiver.SubscribeForV1Trap(subscriber, v1GenericCode, v1SpecificCode, v1EnterpriseFilter, _trapCommunityString);

    }
}
