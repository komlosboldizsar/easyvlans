using System.IO.Pipes;
using System.Security.Principal;

namespace BToolbox.OneInstance;

public class OneInstancePipe
{

    public delegate void ShowMessageReceivedHandler();
    public static event ShowMessageReceivedHandler ShowMessageReceived;

    public static void StartOneInstanceServer(string instanceId)
    {
        _serverInstanceId = instanceId;
        Task.Run(oneInstanceServer);
    }

    private static string _serverInstanceId;

    private static async Task oneInstanceServer()
    {
        while (true)
        {
            NamedPipeServerStream pipeServer = new(OneInstanceSettings.GetObjectName(_serverInstanceId), PipeDirection.In);
            await pipeServer.WaitForConnectionAsync();
            using StreamReader pipeReader = new(pipeServer);
            string pipeMessage = await pipeReader.ReadLineAsync();
            if (pipeMessage == MESSAGE_SHOW)
                ShowMessageReceived?.Invoke();
            pipeServer.Close();
        }
    }

    public static void SignalOtherInstanceToShow(string instanceId)
    {
        using NamedPipeClientStream pipeClient = new(".", OneInstanceSettings.GetObjectName(instanceId), PipeDirection.Out, PipeOptions.Asynchronous, TokenImpersonationLevel.Impersonation);
        pipeClient.Connect(1000);
        StreamWriter pipeWriter = new(pipeClient) { AutoFlush = true };
        pipeWriter.WriteLine(MESSAGE_SHOW);
    }

    private const string MESSAGE_SHOW = "show";

}