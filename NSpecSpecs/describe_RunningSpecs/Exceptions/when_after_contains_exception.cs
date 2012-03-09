using System;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
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
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of after")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("tracks only the first exception from act")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
        }

        [Test]
        public void examples_with_only_after_failure_should_only_fail_because_of_after()
        {
            TheExample("should fail this example because of after")
                .Exception.InnerException.GetType().should_be(typeof(InvalidOperationException));
            TheExample("should also fail this example because of after")
                .Exception.InnerException.GetType().should_be(typeof(InvalidOperationException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_after()
        {
            TheExample("tracks only the first exception from act")
                .Exception.InnerException.GetType().should_be(typeof(ArgumentException));
        }
    }
}