using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using easyvlans.Model.Remote.WebSocket;
using static easyvlans.Model.Remote.WebSocket.MyWebSocketBehavior;

namespace easyvlans.Model.Remote.WebSocket.Helper
{
    class DeserializeCommand
    {
        public command_type command;
        public object data;
    }
}
