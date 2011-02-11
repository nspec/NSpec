using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NSpec
{
    public static class AssertionExtensions
    {
        public static void should_not_be_null(this object o)
        {
            Assert.IsNotNull(o);
        }

        public static void should_be_true(this bool actual)
        {
            Assert.IsTrue(actual);
        }

        public static void should_be_false(this bool actual)
        {
            Assert.IsFalse(actual);
        }

        public static void ShouldBe(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldBe<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            CollectionAssert.AreEqual(expected,actual);
        }
        public static IEnumerable<MethodInfo> Methods(this Type type, IEnumerable<string> exclusions)
        {
            return type.GetMethods().Where(m => !exclusions.Contains(m.Name));
        }
    }
}