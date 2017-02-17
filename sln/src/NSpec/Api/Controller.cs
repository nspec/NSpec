using Newtonsoft.Json;
using NSpec.Api.Discovery;
using NSpec.Api.Execution;
using System;
using System.Collections.Generic;

namespace NSpec.Api
{
    public class Controller
    {
        public int Run(
            string testAssemblyPath,
            string tags,
            string formatterClassName,
            IDictionary<string, string> formatterOptions,
            bool failFast)
        {
            var batchExampleRunner = new BatchExampleRunner(testAssemblyPath,
                tags, formatterClassName, formatterOptions, failFast);

            int nrOfFailures = batchExampleRunner.Start();

            return nrOfFailures;
        }

        public string List(string testAssemblyPath)
        {
            var exampleSelector = new ExampleSelector(testAssemblyPath);

            var discoveredExamples = exampleSelector.Select();

            string serialized = JsonConvert.SerializeObject(discoveredExamples);

            return serialized;
        }

        public void Execute(
            string testAssemblyPath,
            IEnumerable<string> exampleFullNames,
            Action<string> onExampleStarted,
            Action<string> onExampleCompleted)
        {
            Action<DiscoveredExample> onDiscovered = example =>
            {
                string serialized = JsonConvert.SerializeObject(example);

                onExampleStarted(serialized);
            };

            Action<ExecutedExample> onExecuted = example =>
            {
                string serialized = JsonConvert.SerializeObject(example);

                onExampleCompleted(serialized);
            };

            var exampleRunner = new ExampleRunner(testAssemblyPath, onDiscovered, onExecuted);

            exampleRunner.Start(exampleFullNames);
        }
    }
}
