using NSpec.Api.Discovery;
using NSpec.Api.Execution;
using System;
using System.IO;
using System.Reflection;

namespace NSpec.Tests.Api
{
    public static class ApiTestData
    {
        static ApiTestData()
        {
            string thisAssemblyPath = typeof(ApiTestData).GetTypeInfo().Assembly.Location;

            // .NET Core:      go up from test\{Project}\bin\{Config}\{Framework}\{Assembly}.dll
            // .NET Framework: go up from test\{Project}\bin\{Config}\{Framework}\{Platform}\{Assembly}.dll
            string testDirPath = Directory
                .GetParent(thisAssemblyPath)
                .Parent
                .Parent
                .Parent
#if NET451
                .Parent
#endif
                .Parent.FullName;

            string singleTestSourceFilePath = Path.Combine(new[]
            {
                testDirPath,
                "Samples",
                "SampleSpecsApi",
                "desc_SystemUnderTest.cs"
            });

            foreach (var exm in allDiscoveredExamples)
            {
                exm.SourceAssembly = testAssemblyPath;

                if (exm.SourceLineNumber != 0)
                {
                    exm.SourceFilePath = singleTestSourceFilePath;
                }
            }

            TimeSpan nonZeroDuration = new TimeSpan(1, 2, 3);

            foreach (var exm in allExecutedExamples)
            {
                if (!exm.Pending)
                {
                    exm.Duration = nonZeroDuration;
                }
            }
        }

        public static readonly string testAssemblyPath =
            typeof(SampleSpecsApi.PublicPlaceholderClass).GetTypeInfo().Assembly.Location;

        public static readonly DiscoveredExample[] allDiscoveredExamples =
        {
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. method context 1. parent example 1A.",
                SourceLineNumber = 19,
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
                SourceLineNumber = 21,
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
                SourceLineNumber = 26,
                Tags = new[]
                {
                    "ParentSpec",
                },
            },
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.",
                // No source code info available for pending tests
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
                SourceLineNumber = 42,
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
                SourceLineNumber = 49,
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
                SourceLineNumber = 51,
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
                ExceptionStackTrace = "NSpec.AssertionExtensions.ShouldBeFalse(Boolean actual)",
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
