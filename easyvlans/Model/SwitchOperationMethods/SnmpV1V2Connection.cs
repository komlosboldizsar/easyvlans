using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal abstract class SnmpV1V2Connection : ISnmpConnection
    {

        protected readonly IPEndPoint _ipEndPoint;
        protected readonly OctetString _readCommunityString;
        protected readonly OctetString _writeCommunityString;

        public SnmpV1V2Connection(string ip, int port, string communityStrings)
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

        public abstract Task<List<Variable>> WalkAsync(string objectIdentifierStr);
        public abstract Task SetAsync(List<Variable> variables);

    }
}
