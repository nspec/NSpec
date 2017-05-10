using FluentAssertions;
using NSpec.Domain;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Tag("focus")]
    public class describe_expecting_no_exception_in_act : when_expecting_no_exception_in_act
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                it["passes if no exception thrown"] = expectNoExceptions;

                context["when exception thrown from act"] = () =>
                {
                    act = () => { throw new KnownException("unexpected failure"); };

                    it["rethrows the exception from act"] = expectNoExceptions;
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

    }

    public abstract class when_expecting_no_exception_in_act : when_running_specs
    {
        [Test]
        public void should_be_one_failure()
        {
            classContext.Failures().Count().Should().Be(1);
        }

        [Test]
        public void passes_if_no_exception_thrown()
        {
            TheExample("passes if no exception thrown").Exception.Should().BeNull();
        }

        [Test]
        public void rethrows_the_exception_from_act()
        {
            var exception = TheExample("rethrows the exception from act").Exception;
            
            exception.Should().BeOfType<ExampleFailureException>();
            exception.Message.Should().Be("Context Failure: unexpected failure, Example Failure: unexpected failure");
        }
    }
}
