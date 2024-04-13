using BToolbox.SNMP;
using easyvlans.Logger;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;
using System.Net;
using System.Net.Sockets;

namespace easyvlans.Model.Remote.Snmp
{
    public class MySnmpAgent : SnmpAgent, IRemoteMethod
    {

        public string Code => "snmp";

        private const string COMMUNITY_READ_DEFAULT = "public";
        private const string COMMUNITY_WRITE_DEFAULT = "public";

        public MySnmpAgent(int port, string communityRead, string communityWrite, TrapSendingConfig trapSendingConfig)
            : base(port, communityRead ?? COMMUNITY_READ_DEFAULT, communityWrite ?? COMMUNITY_WRITE_DEFAULT, trapSendingConfig)
        { }

        public void MeetConfig(Config config)
        {
            _ = new DataTableBoundObjectStoreAdapter<Switch, SwitchDataTable>(this, config.Switches.Values);
            _ = new DataTableBoundObjectStoreAdapter<Port, PortDataTable>(this, config.Ports);
        }

        public override string OID_BASE => "1.3.6.1.4.1.59150.1";

        protected override void OnSuccessfulStart()
            => SendTraps(TrapIdentifiers.CODE_Started, $"{OID_BASE}.{TrapIdentifiers.EnterpriseBase}", TrapIdentifiers.SPECIFICCODE_Started, null);

    }
}
