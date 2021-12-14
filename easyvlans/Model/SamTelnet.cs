using PrimS.Telnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{

    class SamTelnet : SwitchAccessMode
    {

        private string ip;
        private int port;
        private int DEFAULT_TELNET_PORT = 23;

        Client client;

        public SamTelnet(string ip, int? port)
        {
            this.ip = ip;
            this.port = port ?? DEFAULT_TELNET_PORT;
        }
        public override void Connect()
        {
            if (client != null)
                client.Dispose();
            client = new Client(ip, port, new System.Threading.CancellationToken());
            if (!client.IsConnected)
                throw new CouldNotConnectException();
        }

        public async override void WriteLine(string line) => await client.WriteLine(line);
        public async override Task<string[]> ReadLines()
        {
            string text = await client.ReadAsync();
            return text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

}
