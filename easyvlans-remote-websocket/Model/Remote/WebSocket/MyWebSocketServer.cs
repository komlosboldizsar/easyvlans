using easyvlans.Model.Remote;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace easyvlans.Model.Remote.WebSocket
{
    class MyWebSocketServer : IRemoteMethod
    {

        public string Code => "websocket";
        WebSocketServer _ws;

        private int _port;
        public void Start() {
            _ws.AddWebSocketService<MyWebSocketBehavior>("/easyvlans");
            _ws.Start();


        }

        public MyWebSocketServer(int port)
        {
            
            _port = port;
            
            _ws = new(_port);
        }
        public void MeetConfig(Config config)
        {

           

        }

    }
}
