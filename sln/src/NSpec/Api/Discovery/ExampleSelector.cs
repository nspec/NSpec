using NSpec.Api.Shared;
using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSpec.Api.Discovery
{
    public class ExampleSelector
    {
        public ExampleSelector(string testAssemblyPath)
        {
            this.testAssemblyPath = testAssemblyPath;

            debugInfoProvider = new DebugInfoProvider(testAssemblyPath);
        }

        public IEnumerable<DiscoveredExample> Select()
        {
            var selector = new ContextSelector();

            string noTags = String.Empty;

            selector.Select(testAssemblyPath, noTags);

            var contexts = selector.Contexts;

            var examples = contexts.Examples();

            var discoveredExamples =
                from exm in examples
                select MapToDiscovered(exm, testAssemblyPath);

            return discoveredExamples;
        }

        DiscoveredExample MapToDiscovered(ExampleBase example, string binaryPath)
        {
            var sourceInfo = debugInfoProvider.GetSourceInfo(example);

            var discoveredExample = DiscoveryUtils.MapToDiscovered(example, binaryPath, sourceInfo);

            return discoveredExample;
        }

        readonly string testAssemblyPath;
        readonly DebugInfoProvider debugInfoProvider;
    }
}
