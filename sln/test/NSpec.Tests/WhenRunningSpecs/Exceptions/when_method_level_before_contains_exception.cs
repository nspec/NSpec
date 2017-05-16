using System;
using System.Linq;
using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_method_level_before_contains_exception : when_running_specs
    {
        class MethodBeforeThrowsSpecClass : nspec
        {
            void before_each()
            {
                throw new BeforeEachException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () =>
                {
                    ExamplesRun.Add("should fail");

                    Assert.That("hello", Is.EqualTo("hello"));
                };
            }

            void should_also_fail_this_example()
            {
                it["should also fail"] = () =>
                {
                    ExamplesRun.Add("should also fail");
                    
                    Assert.That("hello", Is.EqualTo("hello"));
                };
            }

            public static List<string> ExamplesRun = new List<string>();
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(MethodBeforeThrowsSpecClass));
        }

        [Test]
        public void examples_should_fail_with_framework_exception()
        {
            classContext.AllExamples().Should().OnlyContain(e => e.Exception is ExampleFailureException);
        }

        [Test]
        public void examples_with_only_before_failure_should_fail_because_of_that()
        {
           classContext.AllExamples().Should().OnlyContain(e => e.Exception.InnerException is BeforeEachException);
        }

        [Test]
        public void examples_should_fail_for_formatter()
        {
            formatter.WrittenExamples.Should().OnlyContain(e => e.Failed);
        }

        [Test]
        public void examples_body_should_not_run()
        {
            MethodBeforeThrowsSpecClass.ExamplesRun.Should().BeEmpty();
        }
    }
}
