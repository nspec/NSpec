using NSpec.Api.Discovery;
using NSpec.Api.Execution;
using System.Reflection;

namespace NSpec.Tests.Api
{
    public static class ApiTestData
    {
        static ApiTestData()
        {
            foreach (var exm in allDiscoveredExamples)
            {
                exm.SourceAssembly = testAssemblyPath;
            }
        }

        public static readonly string testAssemblyPath =
            typeof(SampleSpecsApi.PublicPlaceholderClass).GetTypeInfo().Assembly.Location;

        public static readonly DiscoveredExample[] allDiscoveredExamples =
        {
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. method context 1. parent example 1A.",
                SourceFilePath = "",
                SourceLineNumber = 0,
                Tags = new[]
                {
                    "Tag-1A",
                    "Tag-1B",
                    "ParentSpec",
                },
            },
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. method context 1. parent example 1B.",
                SourceFilePath = "",
                SourceLineNumber = 0,
                Tags = new[]
                {
                    "Tag-1A",
                    "Tag-1B",
                    "ParentSpec",
                },
            },
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. method context 2. parent example 2A.",
                SourceFilePath = "",
                SourceLineNumber = 0,
                Tags = new[]
                {
                    "ParentSpec",
                },
            },
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.",
                SourceFilePath = "",
                SourceLineNumber = 0,
                Tags = new[]
                {
                    "Tag-Child-example-skipped",
                    "Tag-Child",
                    "ChildSpec",
                    "ParentSpec",
                },
            },
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.",
                SourceFilePath = "",
                SourceLineNumber = 0,
                Tags = new[]
                {
                    "Tag with underscores",
                    "Tag-Child",
                    "ChildSpec",
                    "ParentSpec",
                },
            },
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 5. sub context 5-1. child example 5-1A failing.",
                SourceFilePath = "",
                SourceLineNumber = 0,
                Tags = new[]
                {
                    "Tag-Child",
                    "ChildSpec",
                    "ParentSpec",
                },
            },
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 5. sub context 5-1. child example 5-1B.",
                SourceFilePath = "",
                SourceLineNumber = 0,
                Tags = new[]
                {
                    "Tag-Child",
                    "ChildSpec",
                    "ParentSpec",
                },
            },
        };

        public static readonly ExecutedExample[] allExecutedExamples =
        {
            new ExecutedExample()
            {
                FullName = "nspec. ParentSpec. method context 1. parent example 1A.",
                Failed = false,
                Pending = false,
            },
            new ExecutedExample()
            {
                FullName = "nspec. ParentSpec. method context 1. parent example 1B.",
                Failed = false,
                Pending = false,
            },
            new ExecutedExample()
            {
                FullName = "nspec. ParentSpec. method context 2. parent example 2A.",
                Failed = false,
                Pending = false,
            },
            new ExecutedExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.",
                Failed = false,
                Pending = true,
            },
            new ExecutedExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.",
                Failed = false,
                Pending = false,
            },
            new ExecutedExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 5. sub context 5-1. child example 5-1A failing.",
                Failed = true,
                Pending = false,
                ExceptionMessage = "Expected false, but was $True.",
                ExceptionStackTrace = "at NSpec.AssertionExtensions.ShouldBeFalse(Boolean actual)",
            },
            new ExecutedExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 5. sub context 5-1. child example 5-1B.",
                Failed = false,
                Pending = false,
            },
        };
    }
}
