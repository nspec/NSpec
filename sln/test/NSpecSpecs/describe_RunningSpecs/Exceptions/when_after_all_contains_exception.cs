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
    public class when_after_all_contains_exception : when_running_specs
    {
        class AfterAllThrowsSpecClass : nspec
        {
            void method_level_context()
            {
                afterAll = () => { throw new AfterAllException(); };

                it["should fail this example because of afterAll"] = () => Assert.That(true, Is.True);

                it["should also fail this example because of afterAll"] = () => Assert.That(true, Is.True);

                it["overrides exception from same level it"] = () => { throw new ItException(); };

                context["exception thrown by both afterAll and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["preserves exception from nested before"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both afterAll and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["preserves exception from nested act"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both afterAll and nested it"] = () =>
                {
                    it["overrides exception from nested it"] = () => { throw new ItException(); };
                };

                context["exception thrown by both afterAll and nested after"] = () =>
                {
                    it["preserves exception from nested after"] = () => Assert.That(true, Is.True);

                    after = () => { throw new AfterException(); };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(AfterAllThrowsSpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of afterAll")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("should also fail this example because of afterAll")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from same level it")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("preserves exception from nested before")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("preserves exception from nested act")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested it")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("preserves exception from nested after")
                .Exception.Should().BeOfType<ExampleFailureException>();
        }

        [Test]
        public void examples_with_only_after_all_failure_should_fail_because_of_after_all()
        {
            TheExample("should fail this example because of afterAll")
                .Exception.InnerException.Should().BeOfType<AfterAllException>();
            TheExample("should also fail this example because of afterAll")
                .Exception.InnerException.Should().BeOfType<AfterAllException>();
        }

        [Test]
        public void it_should_throw_exception_from_after_all_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.Should().BeOfType<AfterAllException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_before_not_from_after_all()
        {
            TheExample("preserves exception from nested before")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_act_not_from_after_all()
        {
            TheExample("preserves exception from nested act")
                .Exception.InnerException.Should().BeOfType<ActException>();
        }

        [Test]
        public void it_should_throw_exception_from_after_all_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.Should().BeOfType<AfterAllException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_after_not_from_after_all()
        {
            TheExample("preserves exception from nested after")
                .Exception.InnerException.Should().BeOfType<AfterException>();
        }
    }
}
