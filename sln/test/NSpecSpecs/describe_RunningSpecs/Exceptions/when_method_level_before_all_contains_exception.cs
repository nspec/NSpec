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
    public class when_method_level_before_all_contains_exception : when_running_specs
    {
        class MethodBeforeAllThrowsSpecClass : nspec
        {
            void before_all()
            {
                throw new BeforeAllException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () => "hello".Should().Be("hello");
            }

            void should_also_fail_this_example()
            {
                it["should also fail"] = () => "hello".Should().Be("hello");
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(MethodBeforeAllThrowsSpecClass));
        }

        [Test]
        public void the_first_example_should_fail_with_framework_exception()
        {
            var example = classContext.AllExamples().First();

            example.Exception.Should().BeAssignableTo<ExampleFailureException>();
        }

        [Test]
        public void the_second_example_should_fail_with_framework_exception()
        {
            var example = classContext.AllExamples().Last();

            example.Exception.Should().BeAssignableTo<ExampleFailureException>();
        }

        class BeforeAllException : Exception { }
    }
}
