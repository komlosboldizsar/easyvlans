using Lextm.SharpSnmpLib;

namespace easyvlans.Helpers
{
    public static class SnmpVariableNumberConverter
    {

        public static bool ToInt(this Variable variable, Action<int> storeAction)
        {
            if (int.TryParse(variable.Data.ToString(), out int intData))
            {
                storeAction(intData);
                return true;
            }
            return false;
        }

        public static bool ToUInt(this Variable variable, Action<uint> storeAction)
        {
            if (uint.TryParse(variable.Data.ToString(), out uint intData))
            {
                storeAction(intData);
                return true;
            }
            return false;
        }

        public static bool ToTimeTicks(this Variable variable, Action<TimeTicks> storeAction)
            => variable.ToUInt(ui => storeAction(new TimeTicks(ui)));

    }
}
