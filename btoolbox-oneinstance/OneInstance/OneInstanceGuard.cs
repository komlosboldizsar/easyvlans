namespace BToolbox.OneInstance;

public class OneInstanceGuard
{
    public static void Init(string instanceId = null, bool showRunning = true)
    {
        _ = new Mutex(true, OneInstanceSettings.GetObjectName(instanceId), out bool mutexResult);
        if (!mutexResult)
        {
            if (showRunning)
                OneInstancePipe.SignalOtherInstanceToShow(instanceId);
            throw new OneInstanceAlreadyRunningException();
        }
        OneInstancePipe.StartOneInstanceServer(instanceId);
    }
}