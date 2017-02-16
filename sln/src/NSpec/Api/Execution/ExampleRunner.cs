using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Api.Discovery;
using NSpec.Domain;
using NSpec.Domain.Formatters;

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
        }

        public void Start(
            IEnumerable<string> exampleFullNames)
        {
            var selectedNames = new HashSet<string>(exampleFullNames);

            var selector = new ContextSelector();

            string noTags = String.Empty;

            selector.Select(testAssemblyPath, noTags);

            var contexts = selector.Contexts;

            var allExamples = contexts.SelectMany(ctx => ctx.AllExamples());

            var selectedExamples = allExamples.Where(exm => selectedNames.Contains(exm.FullName()));

            var selectedContexts = selectedExamples.Select(exm => exm.Context).Distinct();

            /*
            Func<ExampleBase, bool> isSelected =
                exm => selectedNames.Contains(exm.FullName());

            var selectedContexts = contexts
                .Where(ctx => ctx.Examples.Any(isSelected));
            */

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

            context.Run(this, false, instance);

            context.AssignExceptions();
        }

        readonly string testAssemblyPath;
        readonly Action<DiscoveredExample> onDiscovered;
        readonly Action<ExecutedExample> onExecuted;

        // TODO Extract from ExampleSelector.MapToDiscovered
        static DiscoveredExample MapToDiscovered(ExampleBase example, string binaryPath)
        {
            var discoveredExample = new DiscoveredExample()
            {
                FullName = example.FullName(),
                SourceAssembly = binaryPath,
                // TODO complete with source information
                SourceFilePath = String.Empty,
                SourceLineNumber = 0,
                Tags = example.Tags
                    .Select(tag => tag.Replace("_", " "))
                    .ToArray(),
            };

            return discoveredExample;
        }

        static ExecutedExample MapToExecuted(ExampleBase example)
        {
            var executedExample = new ExecutedExample()
            {
                FullName = example.FullName(),
                Pending = example.Pending,
                Failed = example.Failed(),
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
