using System.Linq;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    public class describe_fail_fast : when_running_specs
    {
        class SpecClass : nspec
        {
            void given_a_spec_with_multiple_failures()
            {
                it["this one isn't a failure"] = () => "not failure".should_be("not failure");

                it["this one is a failure"] = () => "hi".should_be("hello");

                it["this one also fails"] = () => "another".should_be("failure");

                context["nested examples"] = () =>
                {
                    it["is skipped"] = () => "skipped".should_be("skipped");

                    it["is also skipped"] = () => "skipped".should_be("skipped");
                };
            }

            void another_context()
            {
                it["does not run because of failure on line 20"] = () => true.should_be_true();

                it["also does not run because of failure on line 20"] = () => true.should_be_true();
            }
        }

        [SetUp]
        public void Setup()
        {
        	Init(typeof(SpecClass), failFast: true).Run();
        }

        [Test]
        public void only_two_examples_are_executed_one_will_be_a_failure()
        {
            AllExamples().Where(s => s.HasRun).Count().should_be(2);

            TheExample("this one isn't a failure").HasRun.should_be_true();

            TheExample("this one is a failure").HasRun.should_be_true();
        }

        [Test]
        public void only_executed_examples_are_printed()
        {
            formatter.WrittenContexts.First().Name.should_be("SpecClass");

            formatter.WrittenExamples.Count.should_be(2);

            formatter.WrittenExamples.First().FullName().should_contain("this one isn't a failure");

            formatter.WrittenExamples.Last().FullName().should_contain("this one is a failure");
        }
    }
}
