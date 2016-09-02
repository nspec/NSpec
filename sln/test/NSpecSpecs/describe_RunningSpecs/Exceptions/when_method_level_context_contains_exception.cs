using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System;
using System.Linq;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("BareCode")]
    public class when_method_level_context_contains_exception : when_running_specs
    {
        public class MethodContextThrowsSpecClass : nspec
        {
            public void method_level_context_throwing()
            {
                DoSomethingThatThrows();

                before = () => { };

                it["should pass"] = () => { };
            }

            void DoSomethingThatThrows()
            {
                var specEx = new KnownException("Bare code threw exception");

                SpecException = specEx;

                throw specEx;
            }

            public static Exception SpecException;

            public static string ExceptionTypeName = typeof(KnownException).Name;
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(MethodContextThrowsSpecClass));
        }

        [Test]
        public void synthetic_example_name_should_show_exception()
        {
            var example = AllExamples().Single();

            example.FullName().should_contain(MethodContextThrowsSpecClass.ExceptionTypeName);
        }

        [Test]
        public void synthetic_example_should_fail_with_bare_code_exception()
        {
            var example = AllExamples().Single();

            example.Exception.GetType().should_be(typeof(ContextBareCodeException));
        }

        [Test]
        public void bare_code_exception_should_wrap_spec_exception()
        {
            var example = AllExamples().Single();

            example.Exception.InnerException.should_be(MethodContextThrowsSpecClass.SpecException);
        }
    }
}
