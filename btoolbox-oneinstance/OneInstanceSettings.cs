namespace BToolbox.OneInstance;

public class OneInstanceSettings
{

    public static string AppName = "dummyAppName";

    internal static string GetObjectName(string instanceId)
    {
        string pipeName = $"_{KEYWORD_ONEINSTANCE}_{OneInstanceSettings.AppName}";
        if (instanceId != null)
            pipeName += $"_{instanceId}";
        return pipeName;
    }

    public static string KEYWORD_ONEINSTANCE = "ONEINSTANCE";

}