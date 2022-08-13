namespace easyvlans.Model.SwitchOperationMethods
{
    public interface IPersistChangesMethod : ISwitchOperationMethod
    {
        Task DoAsync();
    }
}
