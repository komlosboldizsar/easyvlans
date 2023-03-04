namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISwitchOperationMethodCollection
    {
        IReadConfigMethod ReadConfigMethod { get; }
        ISetPortToVlanMethod SetPortToVlanMethod { get; }
        IPersistChangesMethod PersistChangesMethod { get; }
    }
}
