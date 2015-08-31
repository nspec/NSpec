using System;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_after_all_contains_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                afterAll = () => { throw new AfterAllException(); };

                it["should fail this example because of afterAll"] = () => "1".should_be("1");

                it["should also fail this example because of afterAll"] = () => "1".should_be("1");

                context["exception thrown by both act and afterAll"] = () =>
                {
                    act = () => { throw new ActException("The afterAll's exception should not overwrite the act's exception"); };

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
        [Ignore("ToFix: Exceptions are not registered")]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of afterAll")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of afterAll")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("tracks only the first exception from act")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
        }

        [Test]
        [Ignore("ToFix: Exceptions are not registered")]
        public void examples_with_only_after_all_failure_should_only_fail_because_of_after_all()
        {
            TheExample("should fail this example because of afterAll")
                .Exception.InnerException.GetType().should_be(typeof(AfterAllException));
            TheExample("should also fail this example because of afterAll")
                .Exception.InnerException.GetType().should_be(typeof(AfterAllException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_after_all()
        {
            TheExample("tracks only the first exception from act")
                .Exception.InnerException.GetType().should_be(typeof(ActException));
        }

        class AfterAllException : Exception { }

        class ActException : Exception
        {
            public ActException(string message) : base(message) { }
        }
    }
}
