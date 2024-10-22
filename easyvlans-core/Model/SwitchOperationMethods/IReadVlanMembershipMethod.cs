namespace easyvlans.Model.SwitchOperationMethods
{
    public interface IReadVlanMembershipMethod : ISwitchOperationMethod
    {
        Task DoAsync(IEnumerable<Port> ports = null);
    }
}
