using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_sub_context_contains_exception : when_running_specs
    {
        public class SpecClass : nspec
        {
            public void method_level_context()
            {
                context["sub level context"] = () =>
                {
                    DoSomethingThatThrows();

                    before = () => { };

                    it["should pass"] = () => { };
                };
            }

            void DoSomethingThatThrows()
            {
                throw new KnownException("Bare code threw exception");
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void example_named_after_context_should_fail_with_same_exception()
        {
            var example = TheExample("Context body throws an exception of type KnownException");

            example.Exception.GetType().should_be(typeof(KnownException));
        }
    }
}
