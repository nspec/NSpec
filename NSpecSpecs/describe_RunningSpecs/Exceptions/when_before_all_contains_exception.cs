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

                // just by its presence, this will enforce tests as it should never be reported
                afterAll = () => { throw new AfterAllException(); };

                it["should fail this example because of beforeAll"] = () => "1".should_be("1");

                it["should also fail this example because of beforeAll"] = () => "1".should_be("1");

                it["prevents exception from same level it"] = () => { throw new ItException(); };

                context["exception thrown by both beforeAll and nested before"] = () =>
                {
                    before = () => { throw new BeforeException(); };

                    it["prevents exception from nested before"] = () => "1".should_be("1");
                };

                context["exception thrown by both beforeAll and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["prevents exception from nested act"] = () => "1".should_be("1");
                };

                context["exception thrown by both beforeAll and nested it"] = () =>
                {
                    it["prevents exception from nested it"] = () => { throw new ItException(); };
                };

                context["exception thrown by both beforeAll and nested after"] = () =>
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
            TheExample("should fail this example because of beforeAll")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of beforeAll")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("prevents exception from same level it")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("prevents exception from nested before")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("prevents exception from nested act")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("prevents exception from nested it")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
            TheExample("prevents exception from nested after")
                .Exception.GetType().should_be(typeof(ExampleFailureException));
        }

        [Test]
        public void examples_with_only_before_all_failure_should_fail_because_of_before_all()
        {
            TheExample("should fail this example because of beforeAll")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
            TheExample("should also fail this example because of beforeAll")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_same_level_it()
        {
            TheExample("prevents exception from same level it")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_nested_before()
        {
            TheExample("prevents exception from nested before")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_nested_act()
        {
            TheExample("prevents exception from nested act")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_nested_it()
        {
            TheExample("prevents exception from nested it")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
        }

        [Test]
        public void it_should_throw_exception_from_before_all_not_from_nested_after()
        {
            TheExample("prevents exception from nested after")
                .Exception.InnerException.GetType().should_be(typeof(BeforeAllException));
        }
    }
}
