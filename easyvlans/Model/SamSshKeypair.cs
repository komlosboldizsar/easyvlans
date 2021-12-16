using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    class SamSshKeypair : SwitchAccessMode 
    {

        private string ip;
        private int port;
        private int DEFAULT_SSH_PORT = 22;

        string username;
        string privateKeyFile;

        SshClient client;
        ShellStream shellStream;

        public SamSshKeypair(string ip, int? port, string username, string privateKeyFile)
        {
            this.ip = ip;
            this.port = port ?? DEFAULT_SSH_PORT;
            this.username = username;
            this.privateKeyFile = privateKeyFile;
        }

        public async override Task Connect()
        {
            if (client != null)
            {
                try
                {
                    if (client.IsConnected)
                        client.Disconnect();
                    client.Dispose();
                }
                catch { }
                finally
                {
                    client = null;
                }
            }
            if (shellStream != null)
            {
                try
                {
                    await shellStream.DisposeAsync();
                } catch { }
                finally
                {
                    shellStream = null;
                }
            }
            ConnectionInfo connectionInfo = new ConnectionInfo(ip, port, username, new PrivateKeyAuthenticationMethod(privateKeyFile));
            connectionInfo.Timeout = new TimeSpan(0, 0, 2);
            client = new SshClient(connectionInfo);
            await Task.Run(() => { client.Connect(); });
            if (client?.IsConnected != true)
                throw new CouldNotConnectException();
            shellStream = client.CreateShellStream("easyvlans", 80, 40, 800, 600, 4096);
        }

        public async override Task Authenticate() => await Task.CompletedTask; // suppress warning

        public async override void WriteLine(string line)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(line + "\r\n");
            await shellStream.WriteAsync(bytes, 0, bytes.Length);
        }

        public async override Task<string[]> ReadLines()
        {
            List<string> lines = new List<string>();
            string line;
            do
            {
                line = await Task.Run(() => shellStream.ReadLine(new TimeSpan(0, 0, 0, 100)));
                lines.Add(line);
            } while (line != null);
            return lines.ToArray();
        }

    }
}
