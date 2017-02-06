using NSpec.Domain;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class when_async_before_contains_exception : when_running_specs
    {
        class AsyncBeforeThrowsSpecClass : nspec
        {
            void method_level_context()
            {
                beforeAsync = async () =>
                {
                    await Task.Delay(0);
                    throw new BeforeException();
                };

                it["should fail this example because of beforeAsync"] = () => Assert.That(true, Is.True);

                it["should also fail this example because of beforeAsync"] = () => Assert.That(true, Is.True);

                it["overrides exception from same level it"] = () => { throw new ItException(); };

                context["exception thrown by both beforeAsync and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["overrides exception from nested before"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both beforeAsync and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["overrides exception from nested act"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both beforeAsync and nested it"] = () =>
                {
                    it["overrides exception from nested it"] = () => { throw new ItException(); };
                };

                context["exception thrown by both beforeAsync and nested after"] = () =>
                {
                    it["overrides exception from nested after"] = () => Assert.That(true, Is.True);

                    after = () => { throw new AfterException(); };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(AsyncBeforeThrowsSpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of beforeAsync")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("should also fail this example because of beforeAsync")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from same level it")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested before")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested act")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested it")
                .Exception.Should().BeOfType<ExampleFailureException>();
            TheExample("overrides exception from nested after")
                .Exception.Should().BeOfType<ExampleFailureException>();
        }

        [Test]
        public void examples_with_only_async_before_failure_should_fail_because_of_async_before()
        {
            TheExample("should fail this example because of beforeAsync")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
            TheExample("should also fail this example because of beforeAsync")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_nested_before()
        {
            TheExample("overrides exception from nested before")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_nested_act()
        {
            TheExample("overrides exception from nested act")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_nested_after()
        {
            TheExample("overrides exception from nested after")
                .Exception.InnerException.Should().BeOfType<BeforeException>();
        }
    }
}
