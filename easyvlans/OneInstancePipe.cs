using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans
{
    internal class OneInstancePipe
    {

        public delegate void ShowMessageReceivedHandler();
        public static event ShowMessageReceivedHandler ShowMessageReceived;

        public static void StartOneInstanceServer(string instanceId)
        {
            serverInstanceId = instanceId;
            Task.Run(oneInstanceServer);
        }

        private static string serverInstanceId;

        private static async Task oneInstanceServer()
        {
            while (true)
            {
                NamedPipeServerStream pipeServer = new(getPipeName(serverInstanceId), PipeDirection.In);
                await pipeServer.WaitForConnectionAsync();
                using StreamReader pipeReader = new(pipeServer);
                string pipeMessage = await pipeReader.ReadLineAsync();
                if (pipeMessage == MESSAGE_SHOW)
                    ShowMessageReceived?.Invoke();
                pipeServer.Close();
            }
        }

        public static void SignalOneInstanceToShow(string instanceId)
        {
            using NamedPipeClientStream pipeClient = new(".", getPipeName(instanceId), PipeDirection.Out, PipeOptions.Asynchronous, TokenImpersonationLevel.Impersonation);
            pipeClient.Connect(1000);
            StreamWriter pipeWriter = new(pipeClient) { AutoFlush = true };
            pipeWriter.WriteLine(MESSAGE_SHOW);
        }

        private static string getPipeName(string instanceId)
        {
            string pipeName = ONE_INSTANCE_PIPE_PREFIX;
            if (instanceId != null)
                pipeName += $"_{instanceId}";
            return pipeName;
        }

        private const string MESSAGE_SHOW = "show";
        private const string ONE_INSTANCE_PIPE_PREFIX = "_easyvlans_oneinstance";

    }
}
