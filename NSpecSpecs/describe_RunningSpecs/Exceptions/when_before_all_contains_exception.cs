using System;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_before_all_contains_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                beforeAll = () => { throw new BeforeAllException(); };

                it["should fail this example because of beforeAll"] = () => "1".should_be("1");

                it["should also fail this example because of beforeAll"] = () => "1".should_be("1");

                context["exception thrown by both beforeAll and act"] = () =>
                {
                    act = () => { throw new ActException("this exception should never be thrown"); };

                    it["tracks only the first exception from 'beforeAll'"] = () => "1".should_be("1");
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
            TheExample("should fail this example because of beforeAll")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of beforeAll")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("tracks only the first exception from 'beforeAll'")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
        }

        [Test]
        public void it_should_fail_all_examples_in_before_all()
        {
            TheExample("should fail this example because of beforeAll")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
            TheExample("should also fail this example because of beforeAll")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
        }

        [Test]
        [Ignore("ToFix: Exceptions are not registered")]
        public void it_should_throw_exception_from_before_all_not_from_act()
        {
            TheExample("tracks only the first exception from 'beforeAll'")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
        }

        class BeforeAllException : Exception { }

        class ActException : Exception
        {
            public ActException(string message) : base(message) { }
        }
    }
}
