using FluentAssertions;
using NSpec.Domain;
using NSpec.Tests.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpec.Tests.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_unexpected_exception_in_after : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                context["When same exception thrown in after"] = () =>
                {
                    before = () => { throw new KnownException(); };

                    it["fails because of same exception thrown again in after"] = expect<KnownException>();

                    after = () => { throw new KnownException(); };
                };

                context["When different exception thrown in after"] = () =>
                {
                    before = () => { throw new KnownException(); };

                    it["fails because of different exception thrown in after"] = expect<KnownException>();

                    after = () => { throw new SomeOtherException(); };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void should_fail_because_of_same_exception_in_after()
        {
            var example = TheExample("fails because of same exception thrown again in after");

            example.Exception.Should().NotBeNull();
            example.Exception.Should().BeOfType<ExampleFailureException>();
        }

        [Test]
        public void should_fail_because_of_different_exception_in_after()
        {
            var example = TheExample("fails because of different exception thrown in after");

            example.Exception.Should().NotBeNull();
            example.Exception.Should().BeOfType<ExampleFailureException>();
        }
    }
}
