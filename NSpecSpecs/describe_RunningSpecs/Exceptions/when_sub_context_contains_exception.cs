using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("BareCode")]
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
                var specEx = new KnownException("Bare code threw exception");

                SpecException = specEx;

                throw specEx;
            }

            public static Exception SpecException;
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void example_named_after_context_should_fail_with_bare_code_exception()
        {
            var example = TheExample("Context body throws an exception of type KnownException");

            example.Exception.GetType().should_be(typeof(ContextBareCodeException));
        }

        [Test]
        public void bare_code_exception_should_wrap_spec_exception()
        {
            var example = TheExample("Context body throws an exception of type KnownException");

            example.Exception.InnerException.should_be(SpecClass.SpecException);
        }
    }
}
