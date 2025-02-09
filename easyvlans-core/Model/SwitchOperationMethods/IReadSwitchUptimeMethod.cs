namespace easyvlans.Model.SwitchOperationMethods
{
    public interface IReadSwitchUptimeMethod : ISwitchOperationMethod
    {
        Task DoAsync(Switch @switch);
    }
}
