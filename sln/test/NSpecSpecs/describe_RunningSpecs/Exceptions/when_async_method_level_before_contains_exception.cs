using System;
using System.Linq;
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
    public class when_async_method_level_before_contains_exception : when_running_specs
    {
        class AsyncMethodBeforeThrowsSpecClass : nspec
        {
            async Task before_each()
            {
                await Task.Delay(0);

                throw new BeforeException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () => "hello".Should().Be("hello");
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(AsyncMethodBeforeThrowsSpecClass));
        }

        [Test]
        public void the_example_should_fail_with_ContextFailureException()
        {
            classContext.AllExamples()
                        .First()
                        .Exception
                        .Should().BeAssignableTo<ExampleFailureException>();
        }
    }
}
