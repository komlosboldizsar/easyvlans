using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model.SwitchOperationMethods
{
    public static class FilterArrayHelpers
    {
        public static bool NullOrContains(this string[] haystack, string needle)
            => ((haystack == null) || haystack.Contains(needle));
    }
}
