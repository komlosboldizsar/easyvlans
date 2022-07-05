using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal interface IAccessVlanMembershipMethod
    {
        string Name { get; }
        Task ReadConfigAsync(Switch @switch);
        Task<bool> SetPortToVlanAsync(Switch @switch, UserPort port, UserVlan vlan);
    }
}
