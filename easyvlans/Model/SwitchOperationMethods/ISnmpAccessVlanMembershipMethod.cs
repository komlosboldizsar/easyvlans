namespace easyvlans.Model.SwitchOperationMethods
{
    internal interface ISnmpAccessVlanMembershipMethod : ISnmpMethod, IReadConfigMethod, ISetPortToVlanMethod
    {
        public interface IFactory : IFactory<ISnmpAccessVlanMembershipMethod> { }
    }
}
