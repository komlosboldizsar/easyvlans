namespace easyvlans.Model
{
    internal interface ISnmpAccessVlanMembershipMethod : ISnmpMethod, IReadConfigMethod, ISetPortToVlanMethod
    {
        public interface IFactory : IFactory<ISnmpAccessVlanMembershipMethod> { }
    }
}
