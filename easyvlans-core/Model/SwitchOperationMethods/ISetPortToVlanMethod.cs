namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISetPortToVlanMethod : ISwitchOperationMethod
    {
        Task<bool> DoAsync(Port port, Vlan vlan);
    }
}
