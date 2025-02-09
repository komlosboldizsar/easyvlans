namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISwitchOperationMethodCollection
    {
        IReadSwitchUptimeMethod ReadSwitchUptimeMethod { get; }
        IReadInterfaceStatusMethod ReadInterfaceStatusMethod { get; }
        IReadVlanMembershipMethod ReadVlanMembershipMethod { get; }
        ISetPortToVlanMethod SetPortToVlanMethod { get; }
        IPersistChangesMethod PersistChangesMethod { get; }
    }
}
