namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISwitchOperationMethodCollection
    {
        IReadSwitchBoottimeMethod ReadSwitchBoottimeMethod { get; }
        IReadInterfaceStatusMethod ReadInterfaceStatusMethod { get; }
        IReadVlanMembershipMethod ReadVlanMembershipMethod { get; }
        ISetPortToVlanMethod SetPortToVlanMethod { get; }
        IPersistChangesMethod PersistChangesMethod { get; }
    }
}
