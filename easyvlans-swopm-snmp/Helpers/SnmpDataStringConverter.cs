using Lextm.SharpSnmpLib;

namespace easyvlans.Helpers
{
    internal static class SnmpDataStringConverter
    {
        public static string ToPrettyString(this ISnmpData data)
        {
            switch (data.TypeCode)
            {
                case SnmpType.OctetString:
                    return data.ToBytes().ToHexString();
                case SnmpType.Null:
                    return "<null>";
                default:
                    return data.ToString();
            }
        }
    }
}
