using FluentAssertions;
using FluentAssertions.Equivalency;
using NSpec.Api.Discovery;
using NSpec.Api.Execution;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace NSpec.Tests.Api.Execution
{
    [TestFixture]
    [Category("ExampleRunner")]
    public abstract class describe_ExampleRunner
    {
        protected ExampleRunner runner;

        protected IEnumerable<string> runningTestNames;
        protected List<DiscoveredExample> actualDiscoveredExamples;
        protected List<ExecutedExample> actualExecutedExamples;

        protected readonly string testAssemblyPath = ApiTestData.testAssemblyPath;

        [SetUp]
        public virtual void setup()
        {
            actualDiscoveredExamples = new List<DiscoveredExample>();
            actualExecutedExamples = new List<ExecutedExample>();

            runner = new ExampleRunner(testAssemblyPath, OnDiscovered, OnExecuted);
        }

        void OnDiscovered(DiscoveredExample example)
        {
            actualDiscoveredExamples.Add(example);
        }

        void OnExecuted(ExecutedExample example)
        {
            actualExecutedExamples.Add(example);
        }

        protected EquivalencyAssertionOptions<List<ExecutedExample>> ExpectPartialStackTrace(
            EquivalencyAssertionOptions<List<ExecutedExample>> opts)
        {
            opts.Using<string>(ctx => ctx.Subject?.Should().Contain(ctx.Expectation))
                .When(subj => subj.SelectedMemberPath.EndsWith(".ExceptionStackTrace", System.StringComparison.Ordinal));

            return opts;
        }
    }

    public class when_starting_all_examples : describe_ExampleRunner
    {
        public override void setup()
        {
            base.setup();

            runningTestNames = ApiTestData.allDiscoveredExamples
                .Select(exm => exm.FullName);

            runner.Start(runningTestNames);
        }

        [Test]
        public void it_should_discover_all_examples()
        {
            var expecteds = ApiTestData.allDiscoveredExamples;

            actualDiscoveredExamples.ShouldBeEquivalentTo(expecteds);
        }

        [Test]
        public void it_should_execute_all_examples()
        {
            var expecteds = ApiTestData.allExecutedExamples;

            actualExecutedExamples.ShouldBeEquivalentTo(expecteds, ExpectPartialStackTrace);
        }
    }

    public class when_starting_some_examples : describe_ExampleRunner
    {
        readonly int[] requestedIndexes =
        {
            // method_context_1
            1,
            // method_context_3
            3,
            // method_context_5. sub context 5-1
            5,
        };
        readonly int[] runIndexes =
        {
            // method_context_1
            0, 1,
            // method_context_3
            3,
            // method_context_5. sub context 5-1
            5, 6,
        };

        public override void setup()
        {
            base.setup();

            runningTestNames = ApiTestData.allDiscoveredExamples
                .Where((_, index) => requestedIndexes.Contains(index))
                .Select(exm => exm.FullName);

            runner.Start(runningTestNames);
        }

        [Test]
        public void it_should_discover_sibling_examples()
        {
            var expecteds = ApiTestData.allDiscoveredExamples
                .Where((_, index) => runIndexes.Contains(index));

            actualDiscoveredExamples.ShouldBeEquivalentTo(expecteds);
        }

        [Test]
        public void it_should_execute_sibling_examples()
        {
            var expecteds = ApiTestData.allExecutedExamples
                .Where((_, index) => runIndexes.Contains(index));

            actualExecutedExamples.ShouldBeEquivalentTo(expecteds, ExpectPartialStackTrace);
        }
    }
}
