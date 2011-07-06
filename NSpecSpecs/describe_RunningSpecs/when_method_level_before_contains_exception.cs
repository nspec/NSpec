using System;
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
            Run(typeof(SpecClass), "should_fail_this_example");
        }

        [Test]
        public void it_should_contain_invalid_operation_exception_in_failure_reason()
        {
            classContext.AllExamples()
                        .First()
                        .Exception
                        .should_cast_to<InvalidOperationException>();
        }
    }
}
