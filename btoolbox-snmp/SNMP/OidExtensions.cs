using Lextm.SharpSnmpLib;

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
