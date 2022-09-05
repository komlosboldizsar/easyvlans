namespace easyvlans.Model.Remote.Snmp
{
    internal static class BoolHelpers
    {
        public static int ToSnmpTruthValue(this bool @bool) => @bool ? 1 : 2;
    }
}
