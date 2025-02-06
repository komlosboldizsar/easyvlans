using BToolbox.Logger;
using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Model.Polling
{
    internal static class PollableMethods
    {

        public static readonly string[] POLLABLE_METHOD_CODES =
        {
            MethodCodes.METHOD__READ_INTERFACE_STATUS,
            MethodCodes.METHOD__READ_VLAN_MEMBERSHIP
        };

        public static async Task DoRequest(PollingRequest request)
        {
            Switch @switch = request.Switch;
            switch (request.MethodCode)
            {
                case MethodCodes.METHOD__READ_INTERFACE_STATUS:
                    await @switch.ReadInterfaceStatusAsync();
                    break;
                case MethodCodes.METHOD__READ_VLAN_MEMBERSHIP:
                    await @switch.ReadVlanMembershipAsync();
                    break;
                default:
                    LogDispatcher.E($"Requested poll with unknown method code [{request.MethodCode}] for switch [{@switch.Label}].");
                    break;
            }
        }

    }
}
