using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NSpec
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

        public static void should_not_be(this object actual, object expected)
        {
            Assert.AreNotEqual(expected, actual);
        }

        public static void should_be(this string actual, string expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void should_end_with(this string actual, string end)
        {
            StringAssert.EndsWith(end,actual);
        }

        public static void should_start_with(this string actual, string start)
        {
            StringAssert.StartsWith(start,actual);
        }

        public static void should_contain(this string actual, string expected)
        {
            StringAssert.Contains(expected, actual);
        }

        public static void should_not_contain<T>(this IEnumerable<T> collection, Func<T,bool> predicate)
        {
            Assert.IsTrue( !collection.Any(predicate),"collection contains an item it should not.".With(collection,predicate));
        }

        public static void should_contain<T>(this IEnumerable<T> collection, Func<T,bool> predicate)
        {
            Assert.IsTrue( collection.Any(predicate),"collection does not contain an item it should.".With(collection,predicate));
        }

        public static void should_contain<T>(this IEnumerable<T> collection, T t)
        {
            CollectionAssert.Contains(collection, t);
        }

        public static void should_not_contain<T>(this IEnumerable<T> collection, T t)
        {
            CollectionAssert.DoesNotContain(collection, t);
        }

        public static void should_be_empty<T>(this IEnumerable<T> collection)
        {
            CollectionAssert.IsEmpty(collection);
        }

        public static void should_be<T>(this IEnumerable<T> actual, params T[] expected)
        {
            CollectionAssert.AreEqual(expected.ToArray(),actual.ToArray());
        }

        public static void should_be<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            CollectionAssert.AreEqual(expected.ToArray(),actual.ToArray());
        }

        public static T cast_to<T>(this object valueToCast)
        {
            return (T)valueToCast;
        }
    }
}