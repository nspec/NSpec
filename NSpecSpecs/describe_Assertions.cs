using System;
using NSpec;
using NUnit.Framework;

namespace NSpecSpecs
{
    // PlaceHolder for future
    public class describe_Assertions {}

    [TestFixture]
    public class when_comparing_two_objects
    {
        [Test]
        public void given_timespans_should_be_able_to_assert_on_is_close_to_with_tolerance()
        {
            var thirtyMinutes = new TimeSpan(0, 30, 0);
            var twentyMinutes = new TimeSpan(0, 20, 0);
            var tenMinuteTolerance = new TimeSpan(0, 10, 0);

            thirtyMinutes.is_close_to(twentyMinutes, tenMinuteTolerance);
            thirtyMinutes.should_be_close_to(twentyMinutes, tenMinuteTolerance);
        }

        [Test]
        public void given_datetimes_should_be_able_to_assert_on_is_close_to_with_tolerance()
        {
            var arg1 = new DateTime(2012, 2, 1); // {2/1/2012}
            var arg2 = new DateTime(2012, 2, 2); // {2/2/2012}
            var oneDayTolerance = new DateTime(TimeSpan.TicksPerDay);

            arg1.is_close_to(arg2, oneDayTolerance);
            arg1.should_be_close_to(arg2, oneDayTolerance);
        }

        [Test]
        public void should_be_able_to_assert_on_greater_than()
        {
            2.is_greater_than(1);
            2.should_be_greater_than(1);
        }

        [Test]
        public void should_be_able_to_assert_on_greater_or_equal_to()
        {
            2.is_greater_or_equal_to(2);
            2.should_be_greater_or_equal_to(1);
        }

        [Test]
        public void should_be_able_to_assert_on_less_than()
        {
            2.is_less_than(3);
            2.should_be_less_than(3);
        }

        [Test]
        public void should_be_able_to_assert_on_less_or_equal_to()
        {
            2.is_less_or_equal_to(2);
            2.should_be_less_or_equal_to(3);
        }

        [Test]
        public void given_floats_should_be_able_to_assert_on_is_close_to_with_default_tolerance()
        {
            1e-9f.is_close_to(1e-8f);
            1e-9f.should_be_close_to(1e-8f);
        }

        [Test]
        public void given_floats_should_be_able_to_assert_on_is_close_to_with_custom_tolerance()
        {
            200f.is_close_to(300f, 100f);
            200f.should_be_close_to(300f, 100f);
        }

        [Test]
        public void given_doubles_should_be_able_to_assert_on_is_close_to_with_default_tolerance()
        {
            1e-9.is_close_to(1e-8);
            1e-9.should_be_close_to(1e-8);
        }

        [Test]
        public void given_doubles_should_be_able_to_assert_on_is_close_to_with_custom_tolerance()
        {
            200.0.is_close_to(200.5, .5);
            200.0.should_be_close_to(200.5, .5);
        }
    }
}
