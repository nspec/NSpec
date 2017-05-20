using System.Linq;
using NSpec.Domain;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using System.Collections.Generic;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class when_async_method_level_before_contains_exception : when_running_specs
    {
        class AsyncMethodBeforeThrowsSpecClass : nspec
        {
            async Task before_each()
            {
                await Task.Delay(0);

                throw new BeforeEachException();
            }

            void should_fail_this_example()
            {
                it["should fail"] = () =>
                {
                    ExamplesRun.Add("should fail");
                    Assert.That(true, Is.True);
                };
            }

            void should_also_fail_this_example()
            {
                it["should also fail"] = () =>
                {
                    ExamplesRun.Add("should also fail");
                    Assert.That(true, Is.True);
                };
            }

            public static List<string> ExamplesRun = new List<string>();
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(AsyncMethodBeforeThrowsSpecClass));
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
            AsyncMethodBeforeThrowsSpecClass.ExamplesRun.Should().BeEmpty();
        }
    }
}
