using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;

namespace BToolbox.SNMP
{
    public class TrapSendingConfig
    {

        private DateTime _baseTime;
        private List<TrapReceiver> _receivers = new();
        public int ReceiverCount => _receivers.Count;

        public TrapSendingConfig(DateTime? baseTime = null)
        {
            _baseTime = baseTime ?? DateTime.Now;
        }

        public void SendAll(string code, TrapEnterprise enterprise, IList<Variable> variables)
        {
            uint ticks = (uint)((DateTime.Now - _baseTime).Ticks / 100000);
            foreach (TrapReceiver receiver in _receivers)
                receiver.Send(code, enterprise, ticks, variables ?? EMPTY_VARIABLE_LIST);
        }

        private readonly IList<Variable> EMPTY_VARIABLE_LIST = new List<Variable>();

        public void AddReceiver(string ip, int port, TrapReceiverVersion version, string community, IEnumerable<string> filter, bool sendMyIp)
            => _receivers.Add(new(ip, port, version, community, filter, sendMyIp));

        private class TrapReceiver
        {

            private IPEndPoint _endPoint;
            private TrapReceiverVersion _version;
            private OctetString _community;
            private List<string> _filter;
            private bool _sendMyIp;
            private int _requestId = 1;

            public TrapReceiver(string ip, int port, TrapReceiverVersion version, string community, IEnumerable<string> filter, bool sendMyIp)
            {
                _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                _version = version;
                _community = new(community);
                if (filter != null)
                    _filter = new(filter);
                _sendMyIp = sendMyIp;
            }

            public void Send(string code, TrapEnterprise enterprise, uint ticks, IList<Variable> variables)
            {
                if ((_filter != null) && !_filter.Contains(code))
                    return;
                switch (_version)
                {
                    case TrapReceiverVersion.V1:
                        IPAddress agent = IPAddress.Any;
                        if (_sendMyIp)
                            agent = _endPoint.BestLocalEndPoint().Address;
                        Messenger.SendTrapV1(_endPoint, agent, _community, enterprise.EnterpriseBase, GenericCode.EnterpriseSpecific, enterprise.SpecificCode, ticks, variables);
                        break;
                    case TrapReceiverVersion.V2:
                        Messenger.SendTrapV2(_requestId++, VersionCode.V2, _endPoint, _community, enterprise.EnterpriseFull, ticks, variables);
                        break;
                }
            }

        }

        public enum TrapReceiverVersion
        {
            V1,
            V2
        }


    }
}
