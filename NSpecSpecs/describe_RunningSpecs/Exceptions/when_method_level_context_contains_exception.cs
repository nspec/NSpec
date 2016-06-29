using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_method_level_context_contains_exception : when_running_specs
    {
        public class SpecClass : nspec
        {
            public void method_level_context()
            {
                DoSomethingThatThrows();

                before = () => { };

                it["should pass"] = () => { };
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
            var example = TheExample("method_level_context throws an exception of type KnownException");

            example.Exception.GetType().should_be(typeof(KnownException));
        }
    }
}
