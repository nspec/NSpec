using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpec
{
    public static class AssertionExtensions
    {
        public static void should<T>(this T o, Expression<Predicate<T>> predicate)
        {
            Assert.IsTrue(predicate.Compile()(o), ExampleBase.Parse(predicate.Body));
        }

        public static T should_not_be_null<T>(this T target) where T : class
        {
            Assert.IsNotNull(target);
            return target;
        }

        public static Nullable<T> should_not_be_null<T>(this Nullable<T> target) where T : struct
        {
            Assert.IsTrue(target.HasValue);
            return target;
        }

        public static T should_be_null<T>(this T target) where T : class
        {
            Assert.IsNull(target);
            return target;
        }

        public static Nullable<T> should_be_null<T>(this Nullable<T> target) where T : struct
        {
            Assert.IsFalse(target.HasValue);
            return target;
        }

        public static void should_not_be_default<T>(this T t)
        {
            Assert.AreNotEqual(default(T), t);
        }

        public static void is_not_null_or_empty(this string source)
        {
            Assert.IsNotNull(source);
            Assert.IsNotEmpty(source);
        }

        public static void is_true(this bool actual) { actual.should_be_true(); }
        public static void should_be_true(this bool actual)
        {
            Assert.IsTrue(actual);
        }

        public static void is_false(this bool actual) { actual.should_be_false(); }
        public static void should_be_false(this bool actual)
        {
            Assert.IsFalse(actual);
        }

        public static void should_be(this object actual, object expected)
        {
            actual.Is(expected);
        }

        public static void Is(this object actual, object expected)
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
            StringAssert.EndsWith(end, actual);
        }

        public static void should_start_with(this string actual, string start)
        {
            StringAssert.StartsWith(start, actual);
        }

        public static void should_contain(this string actual, string expected)
        {
            StringAssert.Contains(expected, actual);
        }

        public static IEnumerable<T> should_not_contain<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            Assert.IsTrue(!collection.Any(predicate), "collection contains an item it should not.".With(collection, predicate));

            return collection;
        }

        public static IEnumerable<T> should_contain<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            Assert.IsTrue(collection.Any(predicate), "collection does not contain an item it should.".With(collection, predicate));

            return collection;
        }

        public static IEnumerable<T> should_contain<T>(this IEnumerable<T> collection, T t)
        {
            CollectionAssert.Contains(collection, t);

            return collection;
        }

        public static IEnumerable<T> should_not_contain<T>(this IEnumerable<T> collection, T t)
        {
            CollectionAssert.DoesNotContain(collection, t);

            return collection;
        }

        public static IEnumerable<T> should_not_be_empty<T>(this IEnumerable<T> collection)
        {
            CollectionAssert.IsNotEmpty(collection);

            return collection;
        }

        public static string should_not_be_empty(this string target)
        {
            Assert.IsNotNull(target);
            Assert.IsNotEmpty(target);
            return target;
        }

        public static string should_be_empty(this string target)
        {
            Assert.True(String.IsNullOrEmpty(target));
            return target;
        }

        public static IEnumerable<T> should_be_empty<T>(this IEnumerable<T> collection)
        {
            CollectionAssert.IsEmpty(collection);

            return collection;
        }

        public static IEnumerable<T> should_be<T>(this IEnumerable<T> actual, params T[] expected)
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray());

            return actual;
        }

        public static IEnumerable<T> should_be<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray());

            return actual;
        }

        public static T should_cast_to<T>(this object value)
        {
            Assert.IsInstanceOf<T>(value);
            return (T)value;
        }

        public static void should_not_match(this string value, string pattern)
        {
            StringAssert.DoesNotMatch(pattern, value);
        }

        public static void should_be_same(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
        }

        public static void should_not_be_same(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
        }

        public static void is_greater_than(this IComparable arg1, IComparable arg2){ arg1.should_be_greater_than(arg2);}
        public static void should_be_greater_than(this IComparable arg1, IComparable arg2)
        {
            Assert.Greater(arg1, arg2);
        }

        public static void is_greater_or_equal_to(this IComparable arg1, IComparable arg2){ arg1.should_be_greater_or_equal_to(arg2);}
        public static void should_be_greater_or_equal_to(this IComparable arg1, IComparable arg2)
        {
            Assert.GreaterOrEqual(arg1, arg2);
        }

        public static void is_less_than(this IComparable arg1, IComparable arg2){ arg1.should_be_less_than(arg2);}
        public static void should_be_less_than(this IComparable arg1, IComparable arg2)
        {
            Assert.Less(arg1, arg2);
        }

        public static void is_less_or_equal_to(this IComparable arg1, IComparable arg2){ arg1.should_be_less_or_equal_to(arg2);}
        public static void should_be_less_or_equal_to(this IComparable arg1, IComparable arg2)
        {
            Assert.LessOrEqual(arg1, arg2);
        }

        public static void is_close_to(this float actual, float expected){ actual.should_be_close_to(expected);}
        public static void should_be_close_to(this float actual, float expected)
        {
            actual.should_be_close_to(expected, 0.0000001f);
        }

        public static void is_close_to(this float actual, float expected, float tolerance){ actual.should_be_close_to(expected, tolerance);}
        public static void should_be_close_to(this float actual, float expected, float tolerance)
        {
            Assert.LessOrEqual(Math.Abs(actual - expected), tolerance,
                string.Format("should be close to {0} of {1} but was {2} ", tolerance, expected, actual));
        }

        public static void is_close_to(this double actual, double expected, double tolerance) { actual.should_be_close_to(expected, tolerance); }
        public static void should_be_close_to(this double actual, double expected, double tolerance)
        {
            Assert.LessOrEqual(Math.Abs(actual - expected), tolerance,
                string.Format("should be close to {0} of {1} but was {2}", tolerance, expected, actual));
        }

        public static void is_close_to(this double actual, double expected){ actual.should_be_close_to(expected);}
        public static void should_be_close_to(this double actual, double expected)
        {
            actual.should_be_close_to(expected, 0.0000001f);
        }

        public static void is_close_to(this TimeSpan actual, TimeSpan expected, TimeSpan tolerance){ actual.should_be_close_to(expected, tolerance);}
        public static void should_be_close_to(this TimeSpan actual, TimeSpan expected, TimeSpan tolerance)
        {
            Assert.LessOrEqual(Math.Abs(actual.Ticks - expected.Ticks), tolerance.Ticks,
                string.Format("should be close to {0} ticks of {1} but was {2}", tolerance, expected, actual));
        }

        public static void is_close_to(this DateTime actual, DateTime expected, DateTime tolerance) { actual.should_be_close_to(expected, tolerance);}
        public static void should_be_close_to(this DateTime actual, DateTime expected, DateTime tolerance)
        {
            Assert.LessOrEqual(Math.Abs((actual - expected).Ticks), tolerance.Ticks,
                string.Format("should be close to {0} ticks of {1} but was {2}", tolerance.Ticks, expected, actual));
        }
    }
}