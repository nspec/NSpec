using System;
using NSpec.Domain;
using NUnit.Framework;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using System.Linq;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_method_level_before_contains_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void before_each()
            {
                throw new InvalidOperationException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () => "hello".should_be("hello");
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void the_example_should_fail_with_ContextFailureException()
        {
            classContext.AllExamples()
                        .First()
                        .ExampleLevelException
                        .should_cast_to<ExampleFailureException>();
        }
    }
}
