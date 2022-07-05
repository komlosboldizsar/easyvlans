using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal interface IAccessVlanMembershipMethod : IMethod
    {
        Task ReadConfigAsync(Switch @switch);
        Task<bool> SetPortToVlanAsync(Switch @switch, UserPort port, UserVlan vlan);
    }
}
