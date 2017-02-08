using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSpec.Api.Discovery
{
    public class ExampleSelector
    {
        public IEnumerable<DiscoveredExample> Select(string testAssemblyPath)
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
    }
}
