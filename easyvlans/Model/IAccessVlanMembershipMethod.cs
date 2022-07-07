using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal interface IAccessVlanMembershipMethod : IMethod
    {
        Task ReadConfigAsync();
        Task<bool> SetPortToVlanAsync(UserPort port, UserVlan vlan);
    }
}
