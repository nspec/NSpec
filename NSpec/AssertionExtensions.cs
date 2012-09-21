using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using NSpec.Domain;

namespace NSpec
{
    public static class AssertionExtensions
    {
        public static void should<T>(this T o, Expression<Predicate<T>> predicate)
        {
            NSpecAssert.IsTrue(predicate.Compile()(o), Example.Parse(predicate.Body));
        }

        public static void should_not_be_null(this object o)
        {
            NSpecAssert.IsNotNull(o);
        }

        public static void should_not_be_default<T>(this T t)
        {
            NSpecAssert.AreNotEqual(default(T), t);
        }

        public static void is_not_null_or_empty(this string source)
        {
            NSpecAssert.IsNotNullOrEmpty(source);
        }

        public static void is_true(this bool actual) { actual.should_be_true(); }
        public static void should_be_true(this bool actual)
        {
            NSpecAssert.IsTrue(actual);
        }

        public static void is_false(this bool actual) { actual.should_be_false(); }
        public static void should_be_false(this bool actual)
        {
            NSpecAssert.IsFalse(actual);
        }

        public static void should_be(this object actual, object expected)
        {
            actual.Is(expected);
        }

        public static void Is(this object actual, object expected)
        {
            NSpecAssert.AreEqual(expected, actual);
        }


        public static void should_not_be(this object actual, object expected)
        {
            NSpecAssert.AreNotEqual(expected, actual);
        }

        public static void should_be(this string actual, string expected)
        {
            NSpecAssert.AreEqual(expected, actual);
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
            NSpecAssert.IsTrue(!collection.Any(predicate), "collection contains an item it should not.".With(collection, predicate));

            return collection;
        }

        public static IEnumerable<T> should_contain<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            NSpecAssert.IsTrue(collection.Any(predicate), "collection does not contain an item it should.".With(collection, predicate));

            return collection;
        }

        public static IEnumerable<T> should_contain<T>(this IEnumerable<T> collection, T t)
        {
            NSpecCollectionAssert.Contains(collection, t);

            return collection;
        }

        public static IEnumerable<T> should_not_contain<T>(this IEnumerable<T> collection, T t)
        {
            NSpecCollectionAssert.DoesNotContain(collection, t);

            return collection;
        }

        public static IEnumerable<T> should_not_be_empty<T>(this IEnumerable<T> collection)
        {
            NSpecCollectionAssert.IsNotEmpty(collection);

            return collection;
        }

        public static IEnumerable<T> should_be_empty<T>(this IEnumerable<T> collection)
        {
            NSpecCollectionAssert.IsEmpty(collection);

            return collection;
        }

        public static IEnumerable<T> should_be<T>(this IEnumerable<T> actual, params T[] expected)
        {
            NSpecCollectionAssert.AreEqual(expected.ToArray(), actual.ToArray());

            return actual;
        }

        public static IEnumerable<T> should_be<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            NSpecCollectionAssert.AreEqual(expected.ToArray(), actual.ToArray());

            return actual;
        }

        public static T should_cast_to<T>(this object value)
        {
            NSpecAssert.IsInstanceOf<T>(value);
            return (T)value;
        }

        public static void should_not_match(this string value, string pattern)
        {
            StringAssert.DoesNotMatch(pattern, value);
        }

        public static void should_be_same(this object actual, object expected)
        {
            NSpecAssert.AreSame(expected, actual);
        }

        public static void should_not_be_same(this object actual, object expected)
        {
            NSpecAssert.AreNotSame(expected, actual);
        }

        public static void is_greater_than<T>(this IComparable<T> arg1, IComparable<T> arg2) { arg1.should_be_greater_than(arg2); }
        public static void should_be_greater_than<T>(this IComparable<T> arg1, IComparable<T> arg2)
        {
            NSpecAssert.Greater(arg1, arg2);
        }

        public static void is_greater_or_equal_to<T>(this IComparable<T> arg1, IComparable<T> arg2) { arg1.should_be_greater_or_equal_to(arg2); }
        public static void should_be_greater_or_equal_to<T>(this IComparable<T> arg1, IComparable<T> arg2)
        {
            NSpecAssert.GreaterOrEqual(arg1, arg2);
        }

        public static void is_less_than<T>(this IComparable<T> arg1, IComparable<T> arg2) { arg1.should_be_less_than(arg2); }
        public static void should_be_less_than<T>(this IComparable<T> arg1, IComparable<T> arg2)
        {
            NSpecAssert.Less(arg1, arg2);
        }

        public static void is_less_or_equal_to<T>(this IComparable<T> arg1, IComparable<T> arg2) { arg1.should_be_less_or_equal_to(arg2); }
        public static void should_be_less_or_equal_to<T>(this IComparable<T> arg1, IComparable<T> arg2)
        {
            NSpecAssert.LessOrEqual(arg1, arg2);
        }


        public static void is_close_to(this float actual, float expected){ actual.should_be_close_to(expected);}
        public static void should_be_close_to(this float actual, float expected)
        {
            actual.should_be_close_to(expected, 0.0000001f);
        }

        public static void is_close_to(this float actual, float expected, float tolerance){ actual.should_be_close_to(expected, tolerance);}
        public static void should_be_close_to(this float actual, float expected, float tolerance)
        {
            NSpecAssert.LessOrEqual(Math.Abs(actual - expected), tolerance,
                string.Format("should be close to {0} of {1} but was {2} ", tolerance, expected, actual));
        }

        public static void is_close_to(this double actual, double expected, double tolerance) { actual.should_be_close_to(expected, tolerance); }
        public static void should_be_close_to(this double actual, double expected, double tolerance)
        {
            NSpecAssert.LessOrEqual(Math.Abs(actual - expected), tolerance,
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
            NSpecAssert.LessOrEqual(Math.Abs(actual.Ticks - expected.Ticks), tolerance.Ticks,
                string.Format("should be close to {0} ticks of {1} but was {2}", tolerance, expected, actual));
        }

        public static void is_close_to(this DateTime actual, DateTime expected, DateTime tolerance) { actual.should_be_close_to(expected, tolerance);}
        public static void should_be_close_to(this DateTime actual, DateTime expected, DateTime tolerance)
        {
            NSpecAssert.LessOrEqual(Math.Abs((actual - expected).Ticks), tolerance.Ticks,
                string.Format("should be close to {0} ticks of {1} but was {2}", tolerance.Ticks, expected, actual));
        }
    }

    public static class NSpecCollectionAssert
    {
        public static void Contains<T>(IEnumerable<T> collection, T o)
        {
            collection.Should().Contain(o);
        }

        public static void DoesNotContain<T>(IEnumerable<T> collection, T o)
        {
            collection.Should().NotContain(o);
        }

        public static void IsNotEmpty<T>(IEnumerable<T> collection)
        {
            collection.Should().NotBeEmpty();
        }

        public static void IsEmpty<T>(IEnumerable<T> collection)
        {
            collection.Should().BeEmpty();
        }

        public static void AreEqual<T>(T[] toArray, T[] objects)
        {
            toArray.Should().Equal(objects);
        }
    }

    public static class StringAssert
    {
        public static void EndsWith(string end, string actual)
        {
            actual.Should().EndWith(end);
        }

        public static void StartsWith(string start, string actual)
        {
            actual.Should().StartWith(start);
        }

        public static void Contains(string expected, string actual)
        {
            actual.Should().Contain(expected);
        }

        public static void DoesNotMatch(string pattern, string value)
        {
            value.Should().NotMatch(pattern);
        }
    }

    public static class NSpecAssert
    {
        public static void IsTrue(bool b)
        {
            b.Should().BeTrue();
        }

        public static void IsTrue(bool b, string parse)
        {
            b.Should().BeTrue(parse);
        }

        public static void IsNotNull(object o)
        {
            o.Should().NotBeNull();
        }

        public static void AreNotEqual(object o, object o1)
        {
            o.Should().NotBe(o1);
        }

        public static void IsNotNullOrEmpty(string source)
        {
            source.Should().NotBeNullOrEmpty();
        }

        public static void IsFalse(bool actual)
        {
            actual.Should().BeFalse();
        }

        public static void AreEqual(object expected, object actual)
        {
            actual.Should().Be(expected);
        }

        public static void IsInstanceOf<T>(object value)
        {
            value.Should().BeOfType<T>();
        }

        public static void AreSame(object expected, object actual)
        {
            actual.Should().BeSameAs(expected);
        }

        public static void AreNotSame(object expected, object actual)
        {
            actual.Should().NotBeSameAs(expected);
        }

        public static void Greater<T>(IComparable<T> comparable, IComparable<T> comparable1)
        {
            comparable.Should().BeGreaterThan(comparable1.As<T>());
        }

        public static void GreaterOrEqual<T>(IComparable<T> comparable, IComparable<T> comparable1)
        {
            comparable.Should().BeGreaterOrEqualTo(comparable1.As<T>());
        }

        public static void Less<T>(IComparable<T> comparable, IComparable<T> comparable1)
        {
            comparable.Should().BeLessThan(comparable1.As<T>());
        }

        public static void LessOrEqual<T>(IComparable<T> comparable, IComparable<T> comparable1)
        {
            comparable.Should().BeLessOrEqualTo(comparable1.As<T>());
        }

        public static void LessOrEqual(float comparable, float comparable1, string format)
        {
            comparable.Should().BeLessOrEqualTo(comparable1,format);
        }

        public static void LessOrEqual(double comparable, double comparable1, string format)
        {
            comparable.Should().BeLessOrEqualTo(comparable1,format);
        }
    }
}