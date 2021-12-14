using PrimS.Telnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public async override Task Connect()
        {
            if (client != null)
                client.Dispose();
            CancellationTokenSource connectCancellationTokenSource = new CancellationTokenSource();
            CancellationToken connectCancellationToken = connectCancellationTokenSource.Token;
            await Task.Run(() => {
                try
                {
                    client = new Client(new TcpByteStream(ip, port), connectCancellationToken, new TimeSpan(0, 0, 2));
                }
                catch (InvalidOperationException)
                { }
            });
            if (client?.IsConnected != true)
                throw new CouldNotConnectException();
        }

        public async override Task Authenticate()
        { }

        public async override void WriteLine(string line) => await client.WriteLine(line);
        public async override Task<string[]> ReadLines()
        {
            string text = await client.ReadAsync();
            return text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

}
