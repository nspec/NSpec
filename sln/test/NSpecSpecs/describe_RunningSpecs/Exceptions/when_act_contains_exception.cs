using System;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using FluentAssertions;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_act_contains_exception : when_running_specs
    {
        private class ActThrowsSpecClass : nspec
        {
            void method_level_context()
            {
                act = () => { throw new ActException(); };

                it["should fail this example because of act"] = () => "1".Should().Be("1");

                it["should also fail this example because of act"] = () => "1".Should().Be("1");

                it["overrides exception from same level it"] = () => { throw new ItException(); };

                context["exception thrown by both act and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["preserves exception from nested before"] = () => "1".Should().Be("1");
                };

                context["exception thrown by both act and nested act"] = () =>
                {
                    act = () => { throw new NestedActException(); };

                    it["overrides exception from nested act"] = () => "1".Should().Be("1");
                };

                context["exception thrown by both act and nested it"] = () =>
                {
                    it["overrides exception from nested it"] = () => { throw new ItException(); };
                };

                context["exception thrown by both act and nested after"] = () =>
                {
                    it["overrides exception from nested after"] = () => "1".Should().Be("1");

                    after = () => { throw new AfterException(); };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(ActThrowsSpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of act")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of act")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from same level it")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("preserves exception from nested before")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested act")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested it")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested after")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
        }

        [Test]
        public void examples_with_only_act_failure_should_fail_because_of_act()
        {
            TheExample("should fail this example because of act").Exception
                .InnerException.GetType().Should().Be(typeof(ActException));
            TheExample("should also fail this example because of act").Exception
                .InnerException.GetType().Should().Be(typeof(ActException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.GetType().Should().Be(typeof(ActException));
        }

        [Test]
        public void it_should_throw_exception_from_nested_before_not_from_act()
        {
            TheExample("preserves exception from nested before")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_nested_act()
        {
            TheExample("overrides exception from nested act")
                .Exception.InnerException.GetType().Should().Be(typeof(ActException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.GetType().Should().Be(typeof(ActException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_nested_after()
        {
            TheExample("overrides exception from nested after")
                .Exception.InnerException.GetType().Should().Be(typeof(ActException));
        }
    }
}
