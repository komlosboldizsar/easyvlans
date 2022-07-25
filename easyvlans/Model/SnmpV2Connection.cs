using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal class SnmpV2Connection : ISnmpConnection
    {

        private readonly IPEndPoint _ipEndPoint;
        private readonly OctetString _readCommunityString;
        private readonly OctetString _writeCommunityString;

        public SnmpV2Connection(string ip, int port, string communityStrings)
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            string[] communityStringParts = communityStrings.Split(':');
            if (communityStringParts.Length > 1)
            {
                _readCommunityString = new OctetString(communityStringParts[0]);
                _writeCommunityString = new OctetString(communityStringParts[1]);
            }
            else
            {
                _readCommunityString = _writeCommunityString = new OctetString(communityStrings);
            }
        }

        public async Task<List<Variable>> BulkWalkAsync(string objectIdentifierStr)
        {
            List<Variable> result = new();
            await Messenger.BulkWalkAsync(VersionCode.V2, _ipEndPoint, _readCommunityString, OctetString.Empty,
                new ObjectIdentifier(objectIdentifierStr), result, 5, WalkMode.WithinSubtree, null, null);
            return result;
        }

        public async Task SetAsync(List<Variable> variables)
            => await Messenger.SetAsync(VersionCode.V2, _ipEndPoint, _writeCommunityString, variables);

    }
}
