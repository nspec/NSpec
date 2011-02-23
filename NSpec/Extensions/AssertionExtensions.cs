using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NSpec.Extensions
{
    public static class AssertionExtensions
    {
        public static void should_not_be_null(this object o)
        {
            Assert.IsNotNull(o);
        }

        public static void should_not_be_default<T>(this T t)
        {
            Assert.AreNotEqual(default(T),t);
        }

        public static void is_not_null_or_empty(this string source)
        {
            Assert.IsNotNullOrEmpty(source);
        }

        public static void should_be_true(this bool actual)
        {
            Assert.IsTrue(actual);
        }

        public static void should_be_false(this bool actual)
        {
            Assert.IsFalse(actual);
        }

        public static void should_be_greater_than(this int greater, int lesser)
        {
            Assert.Greater(greater, lesser);
        }

        public static void should_be(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void should_be<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            CollectionAssert.AreEqual(expected.ToArray(),actual.ToArray());
        }
        public static IEnumerable<MethodInfo> Methods(this Type type, IEnumerable<string> exclusions)
        {
            return type.GetMethods().Where(m => !exclusions.Contains(m.Name));
        }
    }
}