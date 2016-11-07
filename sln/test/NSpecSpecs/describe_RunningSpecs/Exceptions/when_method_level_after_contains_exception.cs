using System;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using FluentAssertions;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_method_level_after_contains_exception : when_running_specs
    {
        class MethodAfterThrowsSpecClass : nspec
        {
            void after_each()
            {
                throw new AfterEachException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () => "hello".Should().Be("hello");
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(MethodAfterThrowsSpecClass));
        }

        [Test]
        public void the_example_should_fail_with_framework_exception()
        {
            classContext.AllExamples()
                        .First()
                        .Exception
                        .Should().BeAssignableTo<ExampleFailureException>();
        }

        class AfterEachException : Exception { }
    }
}
