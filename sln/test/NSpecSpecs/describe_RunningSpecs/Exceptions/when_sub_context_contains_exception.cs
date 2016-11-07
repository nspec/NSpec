using FluentAssertions;
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
        public class SubContextThrowsSpecClass : nspec
        {
            public void method_level_context()
            {
                context["sub level context throwing"] = () =>
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
            Run(typeof(SubContextThrowsSpecClass));
        }

        [Test]
        public void synthetic_example_name_should_show_exception()
        {
            var example = AllExamples().Single();

            example.FullName().Should().Contain(SubContextThrowsSpecClass.ExceptionTypeName);
        }

        [Test]
        public void synthetic_example_should_fail_with_bare_code_exception()
        {
            var example = AllExamples().Single();

            example.Exception.GetType().Should().Be(typeof(ContextBareCodeException));
        }

        [Test]
        public void bare_code_exception_should_wrap_spec_exception()
        {
            var example = AllExamples().Single();

            example.Exception.InnerException.Should().Be(SubContextThrowsSpecClass.SpecException);
        }
    }
}
