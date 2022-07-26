namespace easyvlans.Model.SwitchOperationMethods
{
    public interface ISnmpAccessVlanMembershipMethod : ISnmpMethod, IReadConfigMethod, ISetPortToVlanMethod
    {
        public interface IFactory : IFactory<ISnmpAccessVlanMembershipMethod> { }
    }
}
