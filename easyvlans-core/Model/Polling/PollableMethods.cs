using easyvlans.Logger;
using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Model.Polling
{
    internal static class PollableMethods
    {

        public static readonly string[] POLLABLE_METHOD_CODES =
        {
            MethodCodes.METHOD__READ_INTERFACEF_STATUS,
            MethodCodes.METHOD__READ_VLAN_MEMBERSHIP
        };

        public static async Task DoRequest(PollingRequest request)
        {
            Switch @switch = request.Switch;
            switch (request.MethodCode)
            {
                case MethodCodes.METHOD__READ_INTERFACEF_STATUS:
                    break;
                case MethodCodes.METHOD__READ_VLAN_MEMBERSHIP:
                    await @switch.ReadConfigAsync();
                    break;
                default:
                    LogDispatcher.E($"Requested poll with unknown method code [{request.MethodCode}] for switch [{@switch.Label}].");
                    break;
            }
        }

    }
}
