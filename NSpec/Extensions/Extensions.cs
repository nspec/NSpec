using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Domain;

namespace NSpec.Extensions
{
    public static class Extensions
    {
        public static T Instance<T>(this Type type) where T : class
        {
            return type.GetConstructors()[0].Invoke(new object[0]) as T;
        }

        public static IEnumerable<MethodInfo> Methods(this Type type, IEnumerable<string> exclusions)
        {
            return type.GetMethods().Where(m => !exclusions.Contains(m.Name));
        }

        public static string Times(this string source,int times)
        {
            if (times == 0) return "";

            var s = "";

            for (int i = 0; i < times; i++)
                s += source;

            return s;
        }

        public static void Print(this object o)
        {
            Console.WriteLine(o.ToString());
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var t in source)
                action(t);

            return source;
        }

        public static void Do<T, U>(this Tuples<T, U> source, Action<T, U> action)
        {
            foreach (var tup in source)
                action(tup.Item1, tup.Item2);
        }

        public static void Do<T, U>(this Dictionary<T, U> source, Action<T, U> action)
        {
            foreach (var kvp in source)
                action(kvp.Key, kvp.Value);
        }

        public static string With(this string source, params object[] objects)
        {
            var o = Sanitize(objects);
            return string.Format(source, o);
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