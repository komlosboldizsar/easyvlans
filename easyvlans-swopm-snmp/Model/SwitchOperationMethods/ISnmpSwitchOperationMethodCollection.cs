namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISnmpSwitchOperationMethodCollection : ISwitchOperationMethodCollection
    {
        ISnmpConnection SnmpConnection { get; }
        Switch Switch { get; }
    }
}
