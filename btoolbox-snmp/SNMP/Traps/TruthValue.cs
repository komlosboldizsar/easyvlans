using Lextm.SharpSnmpLib;

namespace BToolbox.SNMP
{
    public static class TruthValue
    {
        public const int VALUE_TRUE = 1;
        public const int VALUE_FALSE = 2;
        public static Integer32 Create(bool value) => new(value ? VALUE_TRUE : VALUE_FALSE);
    }
}
