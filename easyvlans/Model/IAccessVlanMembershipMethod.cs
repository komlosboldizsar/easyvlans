using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal interface IAccessVlanMembershipMethod : IMethod
    {
        Task ReadConfigAsync();
        Task<bool> SetPortToVlanAsync(Port port, Vlan vlan);
    }
}
