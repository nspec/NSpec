using System;
using System.Linq;
using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_method_level_after_all_contains_exception : when_running_specs
    {
        class MethodAfterAllThrowsSpecClass : nspec
        {
            void after_all()
            {
                throw new AfterAllException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () => Assert.That("hello", Is.EqualTo("hello"));
            }

            void should_also_fail_this_example()
            {
                it["should also fail"] = () => Assert.That("hello", Is.EqualTo("hello"));
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(MethodAfterAllThrowsSpecClass));
        }

        [Test]
        public void the_first_example_should_fail_with_framework_exception()
        {
            classContext.AllExamples()
                        .First()
                        .Exception
                        .Should().BeAssignableTo<ExampleFailureException>();
        }

        [Test]
        public void the_second_example_should_fail_with_framework_exception()
        {
            classContext.AllExamples()
                        .Last()
                        .Exception
                        .Should().BeAssignableTo<ExampleFailureException>();
        }

        class AfterAllException : Exception { }
    }
}
