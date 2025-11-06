using easyvlans.Model.Remote;
using EmberPlusProviderClassLib;
using EmberPlusProviderClassLib.EmberHelpers;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.Remote.WebSocket
{
    class WebSocketServer : IRemoteMethod
    {

        public string Code => "websocket";

        private int _port;

      

        public void Start() {

           

        }

        public WebSocketServer(int port, string identity, bool autoPersist)
        {
            _port = port;
            _identity = identity;
             _autoPersist = autoPersist;
        }
           
        public void MeetConfig(Config config)
        {

           

        }

    }
}
