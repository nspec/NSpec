using System;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_method_level_after_all_contains_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void after_all()
            {
                throw new AfterAllException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () => "hello".should_be("hello");
            }

            void should_also_fail_this_example()
            {
                it["should also fail"] = () => "hello".should_be("hello");
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        [Ignore("Method-level AfterAll exceptions are not registered")]
        public void the_first_example_should_fail_with_framework_exception()
        {
            classContext.AllExamples()
                        .First()
                        .Exception
                        .should_cast_to<ExampleFailureException>();
        }

        [Test]
        [Ignore("Method-level AfterAll exceptions are not registered")]
        public void the_second_example_should_fail_with_framework_exception()
        {
            classContext.AllExamples()
                        .Last()
                        .Exception
                        .should_cast_to<ExampleFailureException>();
        }

        class AfterAllException : Exception { }
    }
}
