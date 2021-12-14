using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public enum PortStatus
    {
        Unknown,
        VlanRead,
        SettingVlan,
        VlanSetNotPersisted,
        VlanSetFailed,
        VlanSetPersisted
    }
}
