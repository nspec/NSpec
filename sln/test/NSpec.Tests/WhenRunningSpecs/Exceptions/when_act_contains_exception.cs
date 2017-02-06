using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
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

                it["should fail this example because of act"] = () => Assert.That(true, Is.True);

                it["should also fail this example because of act"] = () => Assert.That(true, Is.True);

                it["overrides exception from same level it"] = () => { throw new ItException(); };

                context["exception thrown by both act and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["preserves exception from nested before"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both act and nested act"] = () =>
                {
                    act = () => { throw new NestedActException(); };

                    it["overrides exception from nested act"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both act and nested it"] = () =>
                {
                    it["overrides exception from nested it"] = () => { throw new ItException(); };
                };

                context["exception thrown by both act and nested after"] = () =>
                {
                    it["overrides exception from nested after"] = () => Assert.That(true, Is.True);

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
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("should also fail this example because of act")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from same level it")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("preserves exception from nested before")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested act")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested it")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested after")
                .Exception.Should().BeOfType<ExampleFailureException>();
        }

        [Test]
        public void examples_with_only_act_failure_should_fail_because_of_act()
        {
            TheExample("should fail this example because of act").Exception
                .InnerException.Should().BeOfType<ActException>();
            TheExample("should also fail this example because of act").Exception
                .InnerException.Should().BeOfType<ActException>();
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.Should().BeOfType<ActException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_before_not_from_act()
        {
            TheExample("preserves exception from nested before")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_nested_act()
        {
            TheExample("overrides exception from nested act")
                .Exception.InnerException.Should().BeOfType<ActException>();
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.Should().BeOfType<ActException>();
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_nested_after()
        {
            TheExample("overrides exception from nested after")
                .Exception.InnerException.Should().BeOfType<ActException>();
        }
    }
}
