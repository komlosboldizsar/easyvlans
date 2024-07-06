namespace easyvlans.Model.SwitchOperationMethods
{
    public interface IReadInterfaceStatusMethod : ISwitchOperationMethod
    {
        Task DoAsync(IEnumerable<Port> ports = null);
    }
}
