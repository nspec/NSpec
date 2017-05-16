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
    public class when_method_level_after_all_contains_exception : when_running_specs
    {
        class MethodAfterAllThrowsSpecClass : nspec
        {
            void after_all()
            {
                throw new AfterAllException();
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
            Run(typeof(MethodAfterAllThrowsSpecClass));
        }

        [Test]
        public void examples_should_fail_with_framework_exception()
        {
            classContext.AllExamples().Should().OnlyContain(e => e.Exception is ExampleFailureException);
        }

        [Test]
        public void examples_with_only_after_all_failure_should_fail_because_of_that()
        {
           classContext.AllExamples().Should().OnlyContain(e => e.Exception.InnerException is AfterAllException);
        }

        [Test]
        public void examples_should_fail_for_formatter()
        {
            formatter.WrittenExamples.Should().OnlyContain(e => e.Failed);
        }

        [Test]
        public void examples_body_should_still_run()
        {
            string[] expecteds = new[]
            {
                "should fail",
                "should also fail",
            };

            MethodAfterAllThrowsSpecClass.ExamplesRun.ShouldBeEquivalentTo(expecteds);
        }
    }
}
