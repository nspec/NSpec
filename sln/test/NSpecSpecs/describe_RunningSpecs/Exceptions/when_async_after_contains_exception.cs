using System;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class when_async_after_contains_exception : when_running_specs
    {
        class AsyncAfterThrowsSpecClass : nspec
        {
            void method_level_context()
            {
                afterAsync = async () =>
                {
                    await Task.Delay(0);
                    throw new AfterException();
                };

                it["should fail this example because of afterAsync"] = () => Assert.That(true, Is.True);

                it["should also fail this example because of afterAsync"] = () => Assert.That(true, Is.True);

                it["overrides exception from same level it"] = () => { throw new ItException(); };

                context["exception thrown by both afterAsync and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["preserves exception from nested before"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both afterAsync and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["preserves exception from nested act"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both afterAsync and nested it"] = () =>
                {
                    it["overrides exception from nested it"] = () => { throw new ItException(); };
                };

                context["exception thrown by both afterAsync and nested after"] = () =>
                {
                    it["preserves exception from nested after"] = () => Assert.That(true, Is.True);

                    after = () => { throw new AfterException(); };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(AsyncAfterThrowsSpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of afterAsync")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("should also fail this example because of afterAsync")
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
        public void examples_with_only_after_async_failure_should_fail_because_of_after_async()
        {
            TheExample("should fail this example because of afterAsync")
                .Exception.InnerException.Should().BeOfType<AfterException>();
            TheExample("should also fail this example because of afterAsync")
                .Exception.InnerException.Should().BeOfType<AfterException>();
        }

        [Test]
        public void it_should_throw_exception_from_after_async_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.Should().BeOfType<AfterException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_before_not_from_after_async()
        {
            TheExample("preserves exception from nested before")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_act_not_from_after_async()
        {
            TheExample("preserves exception from nested act")
                .Exception.InnerException.Should().BeOfType<ActException>();
        }

        [Test]
        public void it_should_throw_exception_from_after_async_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.Should().BeOfType<AfterException>();
        }

        [Test]
        public void it_should_throw_exception_from_nested_after_not_from_after_async()
        {
            TheExample("preserves exception from nested after")
                .Exception.InnerException.Should().BeOfType<AfterException>();
        }
    }
}
