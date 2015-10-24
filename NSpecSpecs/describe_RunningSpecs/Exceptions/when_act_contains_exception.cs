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
                act = () => { throw new ActException(); };

                it["should fail this example because of act"] = () => "1".should_be("1");

                it["should also fail this example because of act"] = () => "1".should_be("1");

                it["prevents exception from same level it"] = () => { throw new ItException(); };

                context["exception thrown by both act and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["preserves exception from nested before"] = () => "1".should_be("1");
                };

                context["exception thrown by both act and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["prevents exception from nested act"] = () => "1".should_be("1");
                };

                context["exception thrown by both act and nested it"] = () =>
                {
                    it["prevents exception from nested it"] = () => { throw new ItException(); };
                };

                context["exception thrown by both act and nested after"] = () =>
                {
                    it["prevents exception from nested after"] = () => "1".should_be("1");

                    after = () => { throw new AfterException(); };
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
            TheExample("should fail this example because of act")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of act")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("prevents exception from same level it")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("preserves exception from nested before")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("prevents exception from nested act")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("prevents exception from nested it")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("prevents exception from nested after")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
        }

        [Test]
        public void examples_with_only_act_failure_should_fail_because_of_act()
        {
            TheExample("should fail this example because of act").Exception
                .InnerException.GetType().should_be(typeof(ActException));
            TheExample("should also fail this example because of act").Exception
                .InnerException.GetType().should_be(typeof(ActException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_same_level_it()
        {
            TheExample("prevents exception from same level it")
                .Exception.InnerException.GetType().should_be(typeof(ActException));
        }

        [Test]
        public void it_should_throw_exception_from_nested_before_not_from_act()
        {
            TheExample("preserves exception from nested before")
                .Exception.InnerException.GetType().should_be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_nested_act()
        {
            TheExample("prevents exception from nested act")
                .Exception.InnerException.GetType().should_be(typeof(ActException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_nested_it()
        {
            TheExample("prevents exception from nested it")
                .Exception.InnerException.GetType().should_be(typeof(ActException));
        }

        [Test]
        public void it_should_throw_exception_from_act_not_from_nested_after()
        {
            TheExample("prevents exception from nested after")
                .Exception.InnerException.GetType().should_be(typeof(ActException));
        }
    }
}
