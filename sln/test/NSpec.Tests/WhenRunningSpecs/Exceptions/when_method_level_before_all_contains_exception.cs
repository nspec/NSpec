using System.Linq;
using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    static class MethodBeforeAllThrows
    {
        public class SpecClass : nspec
        {
            void before_all()
            {
                throw new BeforeAllException();
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

        public class ChildSpecClass : SpecClass
        {
            void it_should_fail_because_of_parent()
            {
                ExamplesRun.Add("it_should_fail_because_of_parent");

                Assert.That(true, Is.True);
            }
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class when_method_level_before_all_contains_exception : when_running_specs
    {
        [SetUp]
        public void setup()
        {
            MethodBeforeAllThrows.SpecClass.ExamplesRun.Clear();

            Run(typeof(MethodBeforeAllThrows.SpecClass));
        }

        [Test]
        public void examples_should_fail_with_framework_exception()
        {
            classContext.AllExamples().Should().OnlyContain(e => e.Exception is ExampleFailureException);
        }

        [Test]
        public void examples_with_only_before_all_failure_should_fail_because_of_that()
        {
           classContext.AllExamples().Should().OnlyContain(e => e.Exception.InnerException is BeforeAllException);
        }

        [Test]
        public void examples_should_fail_for_formatter()
        {
            formatter.WrittenExamples.Should().OnlyContain(e => e.Failed);
        }

        [Test]
        public void examples_body_should_not_run()
        {
            MethodBeforeAllThrows.SpecClass.ExamplesRun.Should().BeEmpty();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class when_parent_method_level_before_all_contains_exception : when_running_specs
    {
        [SetUp]
        public void setup()
        {
            MethodBeforeAllThrows.ChildSpecClass.ExamplesRun.Clear();
            
            Run(typeof(MethodBeforeAllThrows.ChildSpecClass));
        }

        [Test]
        public void examples_should_fail_with_framework_exception()
        {
            var example = TheExample("it should fail because of parent");

            example.Exception.Should().BeOfType<ExampleFailureException>();
        }

        [Test]
        public void examples_with_only_before_all_failure_should_fail_because_of_before_all()
        {
            var example = TheExample("it should fail because of parent");

            example.Exception.InnerException.Should().BeOfType<BeforeAllException>();
        }

        [Test]
        public void examples_should_fail_for_formatter()
        {
            formatter.WrittenExamples.Should().OnlyContain(e => e.Failed);
        }

        [Test]
        public void examples_body_should_not_run()
        {
            MethodBeforeAllThrows.ChildSpecClass.ExamplesRun.Should().BeEmpty();
        }
    }
}
