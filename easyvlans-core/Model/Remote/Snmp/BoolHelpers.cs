using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.Remote.Snmp
{
    internal static class BoolHelpers
    {
        public static int ToSnmpTruthValue(this bool @bool) => @bool ? 1 : 2;
    }
}
