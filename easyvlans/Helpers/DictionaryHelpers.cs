using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Helpers
{
    public static class DictionaryHelpers
    {

        public static TValue GetAnyway<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> createDelegate)
        {
            if (!dictionary.TryGetValue(key, out TValue value))
            {
                value = createDelegate(key);
                dictionary.Add(key, value);
            }
            return value;
        }

        public static TValue GetAnyway<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
            => GetAnyway(dictionary, key, k => new TValue());

    }
}
