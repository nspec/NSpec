using Newtonsoft.Json;
using NSpec.Api.Discovery;
using NSpec.Api.Execution;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            var formatter = FindFormatter(formatterClassName, formatterOptions);

            var invocation = new RunnerInvocation(testAssemblyPath, tags, formatter, failFast);

            int nrOfFailures = invocation.Run().Failures().Count();

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

        /// <summary>
        /// Find an implementation of IFormatter with the given class name
        /// </summary>
        /// <param name="formatterClassName"></param>
        /// <param name="formatterOptions"></param>
        /// <returns></returns>
        static IFormatter FindFormatter(string formatterClassName, IDictionary<string, string> formatterOptions)
        {
            // Default formatter is the standard console formatter
            if (string.IsNullOrEmpty(formatterClassName))
            {
                var consoleFormatter = new ConsoleFormatter();
                consoleFormatter.Options = formatterOptions;
                return consoleFormatter;
            }

            Assembly nspecAssembly = typeof(IFormatter).GetTypeInfo().Assembly;

            // Look for a class that implements IFormatter with the provided name
            var formatterType = nspecAssembly.GetTypes().FirstOrDefault(type =>
                (type.Name.ToLowerInvariant() == formatterClassName)
                && typeof(IFormatter).IsAssignableFrom(type));

            if (formatterType != null)
            {
                var formatter = (IFormatter)Activator.CreateInstance(formatterType);
                formatter.Options = formatterOptions;
                return formatter;
            }
            else
            {
                throw new TypeLoadException("Could not find formatter type " + formatterClassName);

            }
        }
    }
}
