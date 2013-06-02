using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpec
{
    /// <summary>
    /// Extension methods for assertions
    /// </summary>
    public static class AssertionExtensions
    {
        /// <summary>
        /// Asserts that the condition is true.
        /// </summary>
        /// <typeparam name="T">The type of object being verified</typeparam>
        /// <param name="o">The object that is the subject of the condition</param>
        /// <param name="predicate">The condition to verify</param>
        public static void should<T>(this T o, Expression<Predicate<T>> predicate)
        {
            Assert.IsTrue(predicate.Compile()(o), Example.Parse(predicate.Body));
        }

        /// <summary>
        /// Verifies that the value is not null.
        /// </summary>
        /// <typeparam name="T">The type of object being verified</typeparam>
        /// <param name="target">The object that should not be null</param>
        /// <returns>The object being verified</returns>
        public static T should_not_be_null<T>(this T target) where T : class
        {
            Assert.IsNotNull(target);
			return target;
        }

        /// <summary>
        /// Verifies that the value is not null.
        /// </summary>
        /// <typeparam name="T">The type of object being verified</typeparam>
        /// <param name="target">The object that should not be null</param>
        /// <returns>The object being verified</returns>
		public static Nullable<T> should_not_be_null<T>(this Nullable<T> target) where T : struct
		{
			Assert.IsTrue(target.HasValue);
			return target;
		}

        /// <summary>
        /// Verifies that the value is null.
        /// </summary>
        /// <typeparam name="T">The type of object being verified</typeparam>
        /// <param name="target">The object that should be null</param>
        /// <returns>The object being verified</returns>
		public static T should_be_null<T>(this T target) where T : class
		{
			Assert.IsNull(target);
			return target;
		}

        /// <summary>
        /// Verifies that the value is null.
        /// </summary>
        /// <typeparam name="T">The type of object being verified</typeparam>
        /// <param name="target">The object that should be null</param>
        /// <returns>The object being verified</returns>
		public static Nullable<T> should_be_null<T>(this Nullable<T> target) where T : struct
		{
			Assert.IsFalse(target.HasValue);
			return target;
		}

        /// <summary>
        /// Verifies that the object is not set to the default value for its datatype.
        /// </summary>
        /// <typeparam name="T">The type of object being verified</typeparam>
        /// <param name="t">The object that should not be set to the default value</param>
        public static void should_not_be_default<T>(this T t)
        {
            Assert.AreNotEqual(default(T), t);
        }

        /// <summary>
        /// Verifies that the string is not null or an empty string.
        /// </summary>
        /// <param name="source">The string to verify</param>
        public static void is_not_null_or_empty(this string source)
        {
            Assert.IsNotNullOrEmpty(source);
        }

        /// <summary>
        /// Verifies that the value is set to true.
        /// </summary>
        /// <param name="actual">The boolean value to verify</param>
        public static void is_true(this bool actual) { actual.should_be_true(); }

        /// <summary>
        /// Verifies that the value is set to true.
        /// </summary>
        /// <param name="actual">The boolean value to verify</param>
        public static void should_be_true(this bool actual)
        {
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Verifies that the value is set to false.
        /// </summary>
        /// <param name="actual">The boolean value to verify</param>
        public static void is_false(this bool actual) { actual.should_be_false(); }

        /// <summary>
        /// Verifies that the value is set to false.
        /// </summary>
        /// <param name="actual">The boolean value to verify</param>
        public static void should_be_false(this bool actual)
        {
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Verifies that the actual value is equal to the given value.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The expected value</param>
        public static void should_be(this object actual, object expected)
        {
            actual.Is(expected);
        }

        /// <summary>
        /// Verifies that the actual value is equal to the given value.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The expected value</param>
        public static void Is(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies that the actual value is not equal to the given value.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The not expected value</param>
        public static void should_not_be(this object actual, object expected)
        {
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        /// Verifies that the actual value is equal to the given value.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The expected value</param>
        public static void should_be(this string actual, string expected)
        {
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies that the string ends with the given string.
        /// </summary>
        /// <param name="actual">The string to verify</param>
        /// <param name="end">The string that it should end with</param>
        public static void should_end_with(this string actual, string end)
        {
            StringAssert.EndsWith(end, actual);
        }

        /// <summary>
        /// Verifies that the string starts with the given string.
        /// </summary>
        /// <param name="actual">The string to verify</param>
        /// <param name="start">The string that it should start with</param>
        public static void should_start_with(this string actual, string start)
        {
            StringAssert.StartsWith(start, actual);
        }

        /// <summary>
        /// Verifies that the given string is found within the original string.
        /// </summary>
        /// <param name="actual">The string to verify</param>
        /// <param name="expected">The string to find</param>
        public static void should_contain(this string actual, string expected)
        {
            StringAssert.Contains(expected, actual);
        }

        /// <summary>
        /// Verifies that the collection does not contain a value that matches the given condition.
        /// </summary>
        /// <typeparam name="T">The type held by the collection</typeparam>
        /// <param name="collection">The collection to search</param>
        /// <param name="predicate">The condition to match on</param>
        /// <returns>The original collection</returns>
        public static IEnumerable<T> should_not_contain<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            Assert.IsTrue(!collection.Any(predicate), "collection contains an item it should not.".With(collection, predicate));

            return collection;
        }

        /// <summary>
        /// Verifies that the collection contains a value that matches the given condition.
        /// </summary>
        /// <typeparam name="T">The type held by the collection</typeparam>
        /// <param name="collection">The collection to search</param>
        /// <param name="predicate">The condition to match on</param>
        /// <returns>The original collection</returns>
        public static IEnumerable<T> should_contain<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            Assert.IsTrue(collection.Any(predicate), "collection does not contain an item it should.".With(collection, predicate));

            return collection;
        }

        /// <summary>
        /// Verifies that the collection contains the given value.
        /// </summary>
        /// <typeparam name="T">The type held by the collection</typeparam>
        /// <param name="collection">The collection to search</param>
        /// <param name="t">The value to search for</param>
        /// <returns>The original collection</returns>
        public static IEnumerable<T> should_contain<T>(this IEnumerable<T> collection, T t)
        {
            CollectionAssert.Contains(collection, t);

            return collection;
        }

        /// <summary>
        /// Verifies that the collection does not contain the given value.
        /// </summary>
        /// <typeparam name="T">The type held by the collection</typeparam>
        /// <param name="collection">The collection to search</param>
        /// <param name="t">The value to search for</param>
        /// <returns>The original collection</returns>
        public static IEnumerable<T> should_not_contain<T>(this IEnumerable<T> collection, T t)
        {
            CollectionAssert.DoesNotContain(collection, t);

            return collection;
        }

        /// <summary>
        /// Verifies that the collection contains atleast one value.
        /// </summary>
        /// <typeparam name="T">The type held by the collection</typeparam>
        /// <param name="collection">The collection to search</param>
        /// <returns>The original collection</returns>
        public static IEnumerable<T> should_not_be_empty<T>(this IEnumerable<T> collection)
        {
            CollectionAssert.IsNotEmpty(collection);

            return collection;
        }

        /// <summary>
        /// Verifies that the string is not null or an empty string.
        /// </summary>
        /// <param name="target">The string to verify</param>
        /// <returns>The original string</returns>
		public static string should_not_be_empty(this string target)
		{
			Assert.IsNotNullOrEmpty(target);
			return target;
		}

        /// <summary>
        /// Verifies that the string is either null or an empty string.
        /// </summary>
        /// <param name="target">The string to verify</param>
        /// <returns>The original string</returns>
		public static string should_be_empty(this string target)
		{
			Assert.IsNullOrEmpty(target);
			return target;
		}

        /// <summary>
        /// Verifies that the collection contains no values.
        /// </summary>
        /// <typeparam name="T">The type held by the collection</typeparam>
        /// <param name="collection">The collection to verify</param>
        /// <returns>The original collection</returns>
        public static IEnumerable<T> should_be_empty<T>(this IEnumerable<T> collection)
        {
            CollectionAssert.IsEmpty(collection);

            return collection;
        }

        /// <summary>
        /// Verifies that the collection contains only the given values.
        /// </summary>
        /// <typeparam name="T">The type held by the collection</typeparam>
        /// <param name="actual">The actual collection</param>
        /// <param name="expected">The expected values</param>
        /// <returns>The original collection</returns>
        public static IEnumerable<T> should_be<T>(this IEnumerable<T> actual, params T[] expected)
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray());

            return actual;
        }

        /// <summary>
        /// Verifies that the actual collection matches the given collection.
        /// </summary>
        /// <typeparam name="T">The type held by the collection</typeparam>
        /// <param name="actual">The actual collection</param>
        /// <param name="expected">The expected collection</param>
        /// <returns>The original collection</returns>
        public static IEnumerable<T> should_be<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray());

            return actual;
        }

        /// <summary>
        /// Verifies that the value can be cast to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to verify</typeparam>
        /// <param name="value">The actual value</param>
        /// <returns>The value cast to the given type</returns>
        public static T should_cast_to<T>(this object value)
        {
            Assert.IsInstanceOf<T>(value);
            return (T)value;
        }

        /// <summary>
        /// Verifies that the actual value does not match the given regular expression pattern.
        /// </summary>
        /// <param name="value">The actual value</param>
        /// <param name="pattern">The regular expression</param>
        public static void should_not_match(this string value, string pattern)
        {
            StringAssert.DoesNotMatch(pattern, value);
        }

        /// <summary>
        /// Verifies that the two objects refer to the same object.
        /// </summary>
        /// <param name="actual">The actual object</param>
        /// <param name="expected">The expected object</param>
        public static void should_be_same(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
        }

        /// <summary>
        /// Verififes that the two objects do not refer to the same object.
        /// </summary>
        /// <param name="actual">The actual object</param>
        /// <param name="expected">The not expected object</param>
        public static void should_not_be_same(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
        }

        /// <summary>
        /// Verifies that the actual value is equal to the given value.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The value that is expected</param>
        public static void should_eql(this object actual, object expected)
        {
            actual.Is(expected);
        }

        /// <summary>
        /// Verifies that the actual value is greater than the given value.
        /// </summary>
        /// <param name="arg1">The actual value</param>
        /// <param name="arg2">The value to compare</param>
        public static void is_greater_than(this IComparable arg1, IComparable arg2){ arg1.should_be_greater_than(arg2);}

        /// <summary>
        /// Verifies that the actual value is greater than the given value.
        /// </summary>
        /// <param name="arg1">The actual value</param>
        /// <param name="arg2">The value to compare</param>
        public static void should_be_greater_than(this IComparable arg1, IComparable arg2)
        {
            Assert.Greater(arg1, arg2);
        }

        /// <summary>
        /// Verifies that the actual value is greater than or equal to the given value.
        /// </summary>
        /// <param name="arg1">The actual value</param>
        /// <param name="arg2">The value to compare</param>
        public static void is_greater_or_equal_to(this IComparable arg1, IComparable arg2){ arg1.should_be_greater_or_equal_to(arg2);}

        /// <summary>
        /// Verifies that the actual value is greater than or equal to the given value.
        /// </summary>
        /// <param name="arg1">The actual value</param>
        /// <param name="arg2">The value to compare</param>
        public static void should_be_greater_or_equal_to(this IComparable arg1, IComparable arg2)
        {
            Assert.GreaterOrEqual(arg1, arg2);
        }

        /// <summary>
        /// Verifies that the actual value is less than the given value.
        /// </summary>
        /// <param name="arg1">The actual value</param>
        /// <param name="arg2">The value to compare</param>
        public static void is_less_than(this IComparable arg1, IComparable arg2){ arg1.should_be_less_than(arg2);}

        /// <summary>
        /// Verifies that the actual value is less than the given value.
        /// </summary>
        /// <param name="arg1">The actual value</param>
        /// <param name="arg2">The value to compare</param>
        public static void should_be_less_than(this IComparable arg1, IComparable arg2)
        {
            Assert.Less(arg1, arg2);
        }

        /// <summary>
        /// Verifies that the actual value is less than or equal to the given value.
        /// </summary>
        /// <param name="arg1">The actual value</param>
        /// <param name="arg2">The value to compare</param>
        public static void is_less_or_equal_to(this IComparable arg1, IComparable arg2){ arg1.should_be_less_or_equal_to(arg2);}

        /// <summary>
        /// Verifies that the actual value is less than or equal to the given value.
        /// </summary>
        /// <param name="arg1">The actual value</param>
        /// <param name="arg2">The value to compare</param>
        public static void should_be_less_or_equal_to(this IComparable arg1, IComparable arg2)
        {
            Assert.LessOrEqual(arg1, arg2);
        }

        /// <summary>
        /// Verifies that the difference between the actual value and the given value is less than 1/10,000,000.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The value to compare</param>
        public static void is_close_to(this float actual, float expected){ actual.should_be_close_to(expected);}

        /// <summary>
        /// Verifies that the difference between the actual value and the given value is less than 1/10,000,000.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The value to compare</param>
        public static void should_be_close_to(this float actual, float expected)
        {
            actual.should_be_close_to(expected, 0.0000001f);
        }

        /// <summary>
        /// Verifies that the difference between the actual value and the given value is less than the tolerance.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The value to compare</param>
        /// <param name="tolerance">The number to act as a tolerance</param>
        public static void is_close_to(this float actual, float expected, float tolerance){ actual.should_be_close_to(expected, tolerance);}

        /// <summary>
        /// Verifies that the difference between the actual value and the given value is less than the tolerance.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The value to compare</param>
        /// <param name="tolerance">The number to act as a tolerance</param>
        public static void should_be_close_to(this float actual, float expected, float tolerance)
        {
            Assert.LessOrEqual(Math.Abs(actual - expected), tolerance,
                string.Format("should be close to {0} of {1} but was {2} ", tolerance, expected, actual));
        }

        /// <summary>
        /// Verifies that the difference between the actual value and the given value is less than the tolerance.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The value to compare</param>
        /// <param name="tolerance">The number to act as a tolerance</param>
        public static void is_close_to(this double actual, double expected, double tolerance) { actual.should_be_close_to(expected, tolerance); }

        /// <summary>
        /// Verifies that the difference between the actual value and the given value is less than the tolerance.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The value to compare</param>
        /// <param name="tolerance">The number to act as a tolerance</param>
        public static void should_be_close_to(this double actual, double expected, double tolerance)
        {
            Assert.LessOrEqual(Math.Abs(actual - expected), tolerance,
                string.Format("should be close to {0} of {1} but was {2}", tolerance, expected, actual));
        }

        /// <summary>
        /// Verifies that the difference between the actual value and the given value is less than 1/10,000,000.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The value to compare</param>
        public static void is_close_to(this double actual, double expected){ actual.should_be_close_to(expected);}

        /// <summary>
        /// Verifies that the difference between the actual value and the given value is less than 1/10,000,000.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <param name="expected">The value to compare</param>
        public static void should_be_close_to(this double actual, double expected)
        {
            actual.should_be_close_to(expected, 0.0000001f);
        }

        /// <summary>
        /// Verifies that the difference between the actual time span and the given time span is less than the tolerance.
        /// </summary>
        /// <param name="actual">The actual time span</param>
        /// <param name="expected">The time span to compare</param>
        /// <param name="tolerance">The time span to act as a tolerance</param>
        public static void is_close_to(this TimeSpan actual, TimeSpan expected, TimeSpan tolerance){ actual.should_be_close_to(expected, tolerance);}

        /// <summary>
        /// Verifies that the difference between the actual time span and the given time span is less than the tolerance.
        /// </summary>
        /// <param name="actual">The actual time span</param>
        /// <param name="expected">The time span to compare</param>
        /// <param name="tolerance">The time span to act as a tolerance</param>
        public static void should_be_close_to(this TimeSpan actual, TimeSpan expected, TimeSpan tolerance)
        {
            Assert.LessOrEqual(Math.Abs(actual.Ticks - expected.Ticks), tolerance.Ticks,
                string.Format("should be close to {0} ticks of {1} but was {2}", tolerance, expected, actual));
        }

        /// <summary>
        /// Verifies that the difference between the actual date/time and the given date/time is less than the tolerance.
        /// </summary>
        /// <param name="actual">The actual date/time</param>
        /// <param name="expected">The date/time to compare</param>
        /// <param name="tolerance">The date/time to act as a tolerance</param>
        public static void is_close_to(this DateTime actual, DateTime expected, DateTime tolerance) { actual.should_be_close_to(expected, tolerance);}

        /// <summary>
        /// Verifies that the difference between the actual date/time and the date/time is less than the tolerance.
        /// </summary>
        /// <param name="actual">The actual date/time</param>
        /// <param name="expected">The date/time to compare</param>
        /// <param name="tolerance">The date/time to act as a tolerance</param>
        public static void should_be_close_to(this DateTime actual, DateTime expected, DateTime tolerance)
        {
            Assert.LessOrEqual(Math.Abs((actual - expected).Ticks), tolerance.Ticks,
                string.Format("should be close to {0} ticks of {1} but was {2}", tolerance.Ticks, expected, actual));
        }
    }
}