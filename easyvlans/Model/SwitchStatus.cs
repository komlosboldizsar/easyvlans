using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public enum SwitchStatus
    {
        NotConnected,
        Connecting,
        CantConnect,
        Authenticating,
        CantAuthenticate,
        Connected,
        NoAccessMode,
        VlansRead,
        PortVlanChanged,
        PortVlanChangeError,
        ConfigSaved,
        ConfigSaveError
    }
}
