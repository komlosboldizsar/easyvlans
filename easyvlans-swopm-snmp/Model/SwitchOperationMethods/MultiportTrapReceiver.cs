using BToolbox.SNMP;
using easyvlans.Helpers;
using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{
    public class MultiportTrapReceiver
    {

        private static readonly Dictionary<int, TrapReceiver> _receivers;

        public static TrapReceiver GetForPort(int port)
        {
            lock (_receivers)
            {
                TrapReceiver receiver = _receivers.GetAnyway(port, p => new TrapReceiver(p));
                if (_initialized)
                    receiver.Start();
                return receiver;
            }
        }

        private static bool _initialized = false;

        public void Init()
        {
            lock (_receivers)
            {
                _initialized = true;
                foreach (TrapReceiver receiver in _receivers.Values)
                    receiver.Start();
            }
        }

    }
}
