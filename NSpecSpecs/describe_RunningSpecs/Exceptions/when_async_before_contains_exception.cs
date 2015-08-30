using System;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class when_async_before_contains_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                asyncBefore = async () => 
                { 
                    await Task.Delay(0);
                    throw new InvalidOperationException(); 
                };

                it["should fail this example because of asyncBefore"] = () => "1".should_be("1");

                it["should also fail this example because of asyncBefore"] = () => "1".should_be("1");

                context["exception thrown by both asyncBefore and act"] = () =>
                {
                    act = () => { throw new ArgumentException("this exception should never be thrown"); };

                    it["tracks only the first exception from 'asyncBefore'"] = () => "1".should_be("1");
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
            TheExample("should fail this example because of asyncBefore")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of asyncBefore")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("tracks only the first exception from 'asyncBefore'")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
        }

        [Test]
        public void it_should_fail_all_examples_in_async_before()
        {
            TheExample("should fail this example because of asyncBefore")
                .Exception.InnerException.GetType().should_be(typeof(InvalidOperationException));
            TheExample("should also fail this example because of asyncBefore")
                .Exception.InnerException.GetType().should_be(typeof(InvalidOperationException));
        }

        [Test]
        public void it_should_throw_exception_from_async_before_not_from_act()
        {
            TheExample("tracks only the first exception from 'asyncBefore'")
                .Exception.InnerException.GetType().should_be(typeof(InvalidOperationException));
        }
    }
}
