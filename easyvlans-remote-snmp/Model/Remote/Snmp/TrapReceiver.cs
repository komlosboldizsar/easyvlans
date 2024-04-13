using BToolbox.SNMP;

namespace easyvlans.Model.Remote.Snmp
{
    internal class TrapReceiver
    {
        public string IP { init; get; }
        public int Port { init; get; }
        public TrapSendingConfig.TrapReceiverVersion Version { init; get; }
        public string Community { init; get; }
        public IEnumerable<string> Filter { init; get; }
        public bool SendMyIp { init; get; }
    }
}
