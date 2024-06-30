namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISwitchOperationMethodCollection
    {
        IReadInterfaceStatusMethod ReadInterfaceStatusMethod { get; }
        IReadConfigMethod ReadConfigMethod { get; }
        ISetPortToVlanMethod SetPortToVlanMethod { get; }
        IPersistChangesMethod PersistChangesMethod { get; }
    }
}
