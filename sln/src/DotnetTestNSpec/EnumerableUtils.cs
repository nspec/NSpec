using System;
using System.Collections.Generic;

namespace DotNetTestNSpec
{
    public static class EnumerableUtils
    {
        public static string ToArrayString<T>(IEnumerable<T> items, bool breakLines = false)
        {
            return ToItemsString("[", "]", items, breakLines);
        }

        public static string ToObjectString<T>(IEnumerable<T> items, bool breakLines = false)
        {
            return ToItemsString("{", "}", items, breakLines);
        }

        public static string ToItemsString<T>(string opening, string closing, IEnumerable<T> items, bool breakLines = false)
        {
            string start = breakLines
                ? "\n"
                : String.Empty;

            string separator = breakLines
                ? ", \n"
                : ", ";

            return String.Concat(opening, " ", start, String.Join(separator, items), " ", closing);
        }
    }
}
