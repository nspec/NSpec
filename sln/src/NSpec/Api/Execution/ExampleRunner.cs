using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Api.Discovery;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NSpec.Api.Shared;

namespace NSpec.Api.Execution
{
    public class ExampleRunner: ILiveFormatter
    {
        public ExampleRunner(
            string testAssemblyPath,
            Action<DiscoveredExample> onDiscovered,
            Action<ExecutedExample> onExecuted)
        {
            this.testAssemblyPath = testAssemblyPath;
            this.onDiscovered = onDiscovered;
            this.onExecuted = onExecuted;

            debugInfoProvider = new DebugInfoProvider(testAssemblyPath);
        }

        public void Start(
            IEnumerable<string> exampleFullNames)
        {
            var selectedNames = new HashSet<string>(exampleFullNames);

            var selector = new ContextSelector();

            string noTags = String.Empty;

            selector.Select(testAssemblyPath, noTags);

            var contexts = selector.Contexts;

            // original idea taken from osoftware/NSpecTestAdapter:
            // see https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Executor.cs

            var allExamples = contexts.SelectMany(ctx => ctx.AllExamples());

            var selectedExamples = allExamples.Where(exm => selectedNames.Contains(exm.FullName()));

            var selectedContexts = selectedExamples.Select(exm => exm.Context).Distinct();

            foreach (var context in selectedContexts)
            {
                RunContext(context);
            }
        }

        // ILiveFormatter

        public void Write(ExampleBase example, int level)
        {
            // ignore level

            var discoveredExample = MapToDiscovered(example, testAssemblyPath);

            onDiscovered(discoveredExample);

            var executedExample = MapToExecuted(example);

            onExecuted(executedExample);
        }

        public void Write(Context context)
        {
            // nothing to do
        }

        void RunContext(Context context)
        {
            // original idea inspired from:
            // https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Executor.cs

            var instance = context.GetInstance();

            context.Run(
                failFast: false,
                instance: instance,
                recurse: false);

            context.AssignExceptions(
                recurse: false);

            context.Write(
                formatter: this,
                recurse: false);
        }

        DiscoveredExample MapToDiscovered(ExampleBase example, string binaryPath)
        {
            var sourceInfo = debugInfoProvider.GetSourceInfo(example);

            var discoveredExample = DiscoveryUtils.MapToDiscovered(example, binaryPath, sourceInfo);

            return discoveredExample;
        }

        readonly string testAssemblyPath;
        readonly Action<DiscoveredExample> onDiscovered;
        readonly Action<ExecutedExample> onExecuted;
        readonly DebugInfoProvider debugInfoProvider;

        static ExecutedExample MapToExecuted(ExampleBase example)
        {
            var executedExample = new ExecutedExample()
            {
                FullName = example.FullName(),
                Pending = example.Pending,
                Failed = example.Failed(),
                Duration = example.Duration,
            };

            if (example.Exception != null)
            {
                executedExample.ExceptionMessage = example.Exception.Message;
                executedExample.ExceptionStackTrace = example.Exception.StackTrace;
            }

            return executedExample;
        }
    }
}
