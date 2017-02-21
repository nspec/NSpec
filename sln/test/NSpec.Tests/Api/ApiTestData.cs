using NSpec.Api.Discovery;
using NSpec.Api.Execution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NSpec.Tests.Api
{
    public static class ApiTestData
    {
        public static readonly string testAssemblyPath =
            typeof(SampleSpecsApi.PublicPlaceholderClass).GetTypeInfo().Assembly.Location;

        public static readonly IEnumerable<DiscoveredExample> allDiscoveredExamples;

        static ApiTestData()
        {
            string sampleSpecsApiProjPath = Path.Combine(new[]
            {
                BuildTestDirectoryPath(),
                "Samples",
                "SampleSpecsApi",
            });

            string descSystemUnderTestFilePath = Path.Combine(new[]
            {
                sampleSpecsApiProjPath,
                "desc_SystemUnderTest.cs",
            });

            string descAsyncSystemUnderTestFilePath = Path.Combine(new[]
            {
                sampleSpecsApiProjPath,
                "desc_AsyncSystemUnderTest.cs",
            });

            var systemUnderTestExampleGroup =
                from exm in descSystemUnderTestDiscoveredExamples
                select new { Example = exm, SourcePath = descSystemUnderTestFilePath };

            var asyncSystemUnderTestExampleGroup =
                from exm in descAsyncSystemUnderTestDiscoveredExamples
                select new { Example = exm, SourcePath = descAsyncSystemUnderTestFilePath };

            allDiscoveredExamples = systemUnderTestExampleGroup
                .Concat(asyncSystemUnderTestExampleGroup)
                .Select(item =>
                {
                    var example = item.Example;

                    // No source code info available for pending tests
                    if (example.SourceLineNumber != 0)
                    {
                        example.SourceFilePath = item.SourcePath;
                    }

                    example.SourceAssembly = testAssemblyPath;

                    return example;
                });

            TimeSpan nonZeroDuration = new TimeSpan(1, 2, 3);

            foreach (var exm in allExecutedExamples)
            {
                if (!exm.Pending)
                {
                    exm.Duration = nonZeroDuration;
                }
            }
        }

        static readonly DiscoveredExample[] descSystemUnderTestDiscoveredExamples =
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
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 5. child example 5A.",
                SourceLineNumber = 54,
                Tags = new[]
                {
                    "Tag-Child",
                    "ChildSpec",
                    "ParentSpec",
                },
            },
            new DiscoveredExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. it child method example A.",
#if DEBUG
                SourceLineNumber = 58,
#endif
#if RELEASE
                SourceLineNumber = 59,
#endif
                Tags = new[]
                {
                    "Tag-Child",
                    "ChildSpec",
                    "ParentSpec",
                },
            },
        };

        static readonly DiscoveredExample[] descAsyncSystemUnderTestDiscoveredExamples =
        {
            new DiscoveredExample()
            {
                FullName = "nspec. AsyncSpec. it async method example.",
#if DEBUG
                SourceLineNumber = 18,
#endif
#if RELEASE
                SourceLineNumber = 19,
#endif
                Tags = new[]
                {
                    "AsyncSpec",
                },
            },
            new DiscoveredExample()
            {
                FullName = "nspec. AsyncSpec. method context. async context example.",
#if DEBUG
                SourceLineNumber = 27,
#endif
#if RELEASE
                SourceLineNumber = 28,
#endif
                Tags = new[]
                {
                    "AsyncSpec",
                },
            },
        };

        public static readonly ExecutedExample[] allExecutedExamples =
        {
            // desc_SystemUnderTest.cs

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
            new ExecutedExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. method context 5. child example 5A.",
                Failed = false,
                Pending = false,
            },
            new ExecutedExample()
            {
                FullName = "nspec. ParentSpec. ChildSpec. it child method example A.",
                Failed = false,
                Pending = false,
            },

            // desc_AsyncSystemUnderTest.cs

            new ExecutedExample()
            {
                FullName = "nspec. AsyncSpec. it async method example.",
                Failed = false,
                Pending = false,
            },
            new ExecutedExample()
            {
                FullName = "nspec. AsyncSpec. method context. async context example.",
                Failed = false,
                Pending = false,
            },
        };

        static string BuildTestDirectoryPath()
        {
            string thisAssemblyPath = typeof(ApiTestData).GetTypeInfo().Assembly.Location;

            // .NET Framework: go up from test\{Project}\bin\{Config}\{Framework}\{Platform}\{Assembly}.dll
            // .NET Core:      go up from test\{Project}\bin\{Config}\{Framework}\{Assembly}.dll
            string testDirPath = Directory
                .GetParent(thisAssemblyPath)
#if NET451
                .Parent
#endif
                .Parent
                .Parent
                .Parent
                .Parent.FullName;

            return testDirPath;
        }
    }
}
