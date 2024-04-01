using Lextm.SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.SNMP
{
    internal static class OidExtensions
    {
        public static ObjectIdentifier Shorten(this ObjectIdentifier objectIdentifier)
        {
            uint[] oid = objectIdentifier.ToNumerical();
            return new ObjectIdentifier(oid.Take(oid.Length - 1).ToArray());
        }
    }
}
