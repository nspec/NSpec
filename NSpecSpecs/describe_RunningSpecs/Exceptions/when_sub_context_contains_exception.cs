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

            public static string ExceptionTypeName = typeof(KnownException).Name;
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void synthetic_example_name_should_show_exception()
        {
            var example = FindSyntheticExample();

            example.should_not_be_null();
        }

        [Test]
        public void synthetic_example_should_fail_with_bare_code_exception()
        {
            var example = FindSyntheticExample();

            example.Exception.GetType().should_be(typeof(ContextBareCodeException));
        }

        [Test]
        public void bare_code_exception_should_wrap_spec_exception()
        {
            var example = FindSyntheticExample();

            example.Exception.InnerException.should_be(SpecClass.SpecException);
        }

        ExampleBase FindSyntheticExample()
        {
            var filteredExamples =
                from exm in AllExamples()
                let fullname = exm.FullName()
                where fullname.Contains(SpecClass.ExceptionTypeName)
                select exm;

            var example = filteredExamples.FirstOrDefault();

            return example;
        }
    }
}
