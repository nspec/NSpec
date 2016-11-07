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
    public class when_spec_class_ctor_contains_exception : when_running_specs
    {
        public class CtorThrowsSpecClass : nspec
        {
            readonly object someTestObject = DoSomethingThatThrows();

            public void method_level_context()
            {
                before = () => { };

                it["should pass"] = () => { };
            }

            static object DoSomethingThatThrows()
            {
                var specEx = new KnownException("Bare code threw exception");

                SpecException = specEx;

                throw specEx;
            }

            public static Exception SpecException;

            public static string TypeFullName = typeof(CtorThrowsSpecClass).FullName;
            public static string ExceptionTypeName = typeof(KnownException).Name;
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(CtorThrowsSpecClass));
        }

        [Test]
        public void synthetic_example_name_should_show_class_and_exception()
        {
            var example = AllExamples().Single();

            example.FullName().Should().Contain(CtorThrowsSpecClass.TypeFullName);

            example.FullName().Should().Contain(CtorThrowsSpecClass.ExceptionTypeName);
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

            example.Exception.InnerException.Should().Be(CtorThrowsSpecClass.SpecException);
        }
    }
}
