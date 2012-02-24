using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace NSpec
{
    public static class Extensions
    {
        /// <summary>
        /// string will be repeated n number of times.
        /// </summary>
        [DebuggerNonUserCode]
        public static string Times(this string source, int times)
        {
            if (times == 0) return "";

            var s = "";

            for (int i = 0; i < times; i++)
                s += source;

            return s;
        }

        /// <summary>
        /// Action(T) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be executed across a common set of examples.
        /// </summary>
        [DebuggerNonUserCode]
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var t in source)
                action(t);

            return source;
        }

        /// <summary>
        /// Action(T, T) will get executed for each consecutive 2 items in a list.  You can use this to specify a suite of data that needs to be executed across a common set of examples.
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static IEnumerable<T> EachConsecutive2<T>(this IEnumerable<T> source, Action<T, T> action)
        {
            var array = source.ToArray();
            for (int i = 0; i < array.Length - 1; i++)
            {
                action(array[i], array[i + 1]);
            }

            return source;
        }

        /// <summary>
        /// Action(T, U) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be execute across a common set of examples.
        /// </summary>
        [DebuggerNonUserCode]
        public static void Do<T, U>(this Each<T, U> source, Action<T, U> action)
        {
            foreach (var tup in source)
                action(tup.Item1, tup.Item2);
        }

        /// <summary>
        /// Action(T, U) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be execute across a common set of examples.
        /// </summary>
        [DebuggerNonUserCode]
        public static void Do<T, U>(this Dictionary<T, U> source, Action<T, U> action)
        {
            foreach (var kvp in source)
                action(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Action(T, U, V) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be execute across a common set of examples.
        /// </summary>
        [DebuggerNonUserCode]
        public static void Do<T, U, V>(this Each<T, U, V> source, Action<T, U, V> action)
        {
            foreach (var tup in source)
                action(tup.Item1, tup.Item2, tup.Item3);
        }

        /// <summary>
        /// Action(T, U, V, W) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be execute across a common set of examples.
        /// </summary>
        [DebuggerNonUserCode]
        public static void Do<T, U, V, W>(this Each<T, U, V, W> source, Action<T, U, V, W> action)
        {
            foreach (var tup in source)
                action(tup.Item1, tup.Item2, tup.Item3, tup.Item4);
        }

        /// <summary>
        /// Action will be executed n number of times.
        /// </summary>
        [DebuggerNonUserCode]
        public static void Times(this int number, Action action)
        {
            for (int i = 1; i <= number; i++)
            {
                action();
            }
        }

        /// <summary>
        /// Flattens an Enumerable&lt;string&gt; into one string with optional separator
        /// </summary>
        [DebuggerNonUserCode]
        public static string Flatten(this IEnumerable<string> source, string separator = "")
        {
            return source.Aggregate((acc, s) => acc = acc + separator + s);
        }

        /// <summary>
        /// Safely access a property or method of type T. If it is null or throws
        /// the fallback will be used instead
        /// </summary>
        [DebuggerNonUserCode]
        public static U GetOrFallback<T, U>(this T t, Func<T, U> func, U fallback)
        {
            try
            {
                if (func(t) == null)
                    return fallback;

                return func(t);
            }
            catch
            {
                return default(U);
            }
        }

        /// <summary>
        /// Create an IEnumerable&lt;int&gt; range from x to y
        /// eg. 1.To(3) would be [1,2,3]
        /// </summary>
        [DebuggerNonUserCode]
        public static IEnumerable<int> To(this int start, int end)
        {
            for (int i = start; i <= end; i++)
                yield return i;
        }

        /// <summary>
        /// Extension method that wraps String.Format.
        /// <para>Usage: string result = "{0} {1}".With("hello", "world");</para>
        /// </summary>
        [DebuggerNonUserCode]
        public static string With(this string source, params object[] objects)
        {
            var o = Sanitize(objects);
            return string.Format(source, o);
        }

        public static void SafeInvoke<T>(this Action<T> action, T t)
        {
            if (action != null) action(t);
        }

        public static void SafeInvoke(this Action action)
        {
            if (action != null) action();
        }

        public static string[] Sanitize(this object[] source)
        {
            return source.ToList().Select(o =>
            {
                if (o.GetType().Equals(typeof(int[])))
                {
                    var s = "";

                    (o as int[]).Do(i => s += i + ",");

                    if (s == "")
                        return "[]";

                    return "[" + s.Remove(s.Length - 1, 1) + "]";
                }

                return o.ToString();
            }).ToArray();
        }
    }
}