using System.Text;

namespace easyvlans.Helpers
{
    internal static class HexStringConverter
    {
        // @source https://stackoverflow.com/a/311179
        public static string ToHexString(this byte[] ba)
        {
            StringBuilder hex = new(ba.Length * 3);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2} ", b);
            if (hex.Length > 0)
                hex.Remove(hex.Length - 1, 1);
            return hex.ToString();
        }
    }
}
