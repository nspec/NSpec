using System;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_act_contains_exception : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                act = () => { throw new InvalidOperationException(); };

                it["should fail this example because of act"] = () => "1".should_be("1");

                it["should also fail this example because of act"] = () => "1".should_be("1");
            }
        }

        [SetUp]
        public void setup()
        {
            Init(typeof(SpecClass));

            Run();
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of act")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of act")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
        }

        [Test]
        public void it_should_fail_all_examples_in_act()
        {
            TheExample("should fail this example because of act").Exception
                .InnerException.GetType().should_be(typeof(InvalidOperationException));
            TheExample("should also fail this example because of act").Exception
                .InnerException.GetType().should_be(typeof(InvalidOperationException));
        }
    }
}
