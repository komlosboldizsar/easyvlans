namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISwitchOperationMethodCollection
    {
        IReadInterfaceStatusMethod ReadInterfaceStatusMethod { get; }
        IReadVlanMembershipMethod ReadVlanMembershipMethod { get; }
        ISetPortToVlanMethod SetPortToVlanMethod { get; }
        IPersistChangesMethod PersistChangesMethod { get; }
    }
}
