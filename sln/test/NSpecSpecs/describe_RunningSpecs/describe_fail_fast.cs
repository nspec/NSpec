using System.Linq;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using FluentAssertions;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    public class describe_fail_fast : when_running_specs
    {
        class SpecClass : nspec
        {
            void given_a_spec_with_multiple_failures()
            {
                it["this one isn't a failure"] = () => "not failure".Should().Be("not failure");

                it["this one is a failure"] = () => "hi".Should().Be("hello");

                it["this one also fails"] = () => "another".Should().Be("failure");

                context["nested examples"] = () =>
                {
                    it["is skipped"] = () => "skipped".Should().Be("skipped");

                    it["is also skipped"] = () => "skipped".Should().Be("skipped");
                };
            }

            void another_context()
            {
                it["does not run because of failure on line 20"] = () => true.Should().BeTrue();

                it["also does not run because of failure on line 20"] = () => true.Should().BeTrue();
            }
        }

        [SetUp]
        public void Setup()
        {
            failFast = true;
            Run(typeof(SpecClass));
        }

        [Test]
        public void should_skip()
        {
            TheExample("is skipped").HasRun.Should().BeFalse();
        }

        [Test]
        public void only_two_examples_are_executed_one_will_be_a_failure()
        {
            AllExamples().Where(s => s.HasRun).Count().Should().Be(2);

            TheExample("this one isn't a failure").HasRun.Should().BeTrue();

            TheExample("this one is a failure").HasRun.Should().BeTrue();
        }

        [Test]
        public void only_executed_examples_are_printed()
        {
            formatter.WrittenContexts.First().Name.Should().Be("SpecClass");

            formatter.WrittenExamples.Count.Should().Be(2);

            formatter.WrittenExamples.First().FullName().Should().Contain("this one isn't a failure");

            formatter.WrittenExamples.Last().FullName().Should().Contain("this one is a failure");
        }
    }
}
