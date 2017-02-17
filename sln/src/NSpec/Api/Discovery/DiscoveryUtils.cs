using Microsoft.Extensions.Testing.Abstractions;
using NSpec.Domain;
using System.Linq;

namespace NSpec.Api.Discovery
{
    public static class DiscoveryUtils
    {
        public static DiscoveredExample MapToDiscovered(ExampleBase example, string binaryPath, SourceInformation sourceInfo)
        {
            var discoveredExample = new DiscoveredExample()
            {
                FullName = example.FullName(),
                SourceAssembly = binaryPath,
                SourceFilePath = sourceInfo.Filename,
                SourceLineNumber = sourceInfo.LineNumber,
                Tags = example.Tags
                    .Select(tag => tag.Replace("_", " "))
                    .ToArray(),
            };

            return discoveredExample;
        }
    }
}
