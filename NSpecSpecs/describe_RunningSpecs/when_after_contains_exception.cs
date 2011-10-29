using System;
using System.Linq;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_after_contains_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                after = () => { throw new InvalidOperationException(); };

                it["should fail this example because of after"] = () => "1".should_be("1");

                it["should also fail this example because of after"] = () => "1".should_be("1");

                context["exception thrown by both act and after"] = () =>
                {
                    act = () => { throw new ArgumentException("The after's exception should not overwrite the act's exception"); };

                    it["tracks only the first exception from act"] = () => "1".should_be("1");
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of after")
                .ExampleLevelException.GetType().should_be(typeof(ContextFailureException));
            TheExample("should also fail this example because of after")
                .ExampleLevelException.GetType().should_be(typeof(ContextFailureException));
            TheExample("tracks only the first exception from act")
                .ExampleLevelException.GetType().should_be(typeof(ContextFailureException));
        }

        [Test]
        public void examples_with_only_after_failure_should_only_fail_because_of_after()
        {
            TheExample("should fail this example because of after")
                .ExampleLevelException.InnerException.GetType().should_be(typeof(InvalidOperationException));
            TheExample("should also fail this example because of after")
                .ExampleLevelException.InnerException.GetType().should_be(typeof(InvalidOperationException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_after()
        {
            TheExample("tracks only the first exception from act")
                .ExampleLevelException.InnerException.GetType().should_be(typeof(ArgumentException));
        }
    }
}