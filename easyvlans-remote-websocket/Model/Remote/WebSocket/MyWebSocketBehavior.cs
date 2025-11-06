using easyvlans.Model.Remote.WebSocket.Helper;
using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace easyvlans.Model.Remote.WebSocket
{
    internal class MyWebSocketBehavior : WebSocketBehavior
    {
        public enum command_type { PING };
        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            var chars = e.Data.ToCharArray();
            Send(new String(chars));

            DeserializeCommand message = JsonNet.Deserialize<DeserializeCommand>(e.Data);

            switch (message.command)
            {
                case command_type.PING:
                    Send("{PONG:1}");
                    break;
                
            }
        }
    }
}
