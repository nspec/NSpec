using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec
{
    public static class DictionaryUtils
    {
        public static string ToArrayString<K, V>(IDictionary<K, V> items, bool breakLines = false)
        {
            return ToItemsString("[", "]", items, breakLines);
        }

        public static string ToObjectString<K, V>(IDictionary<K, V> items, bool breakLines = false)
        {
            return ToItemsString("{", "}", items, breakLines);
        }

        static string ToItemsString<K, V>(string opening, string closing, IDictionary<K, V> items, bool breakLines = false)
        {
            IEnumerable<string> textItems = items.Select(pair => $"{pair.Key}: {pair.Value}");

            return EnumerableUtils.ToItemsString(opening, closing, textItems, breakLines);
        }
    }
}
