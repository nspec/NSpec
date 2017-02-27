using Microsoft.DotNet.ProjectModel;
using Microsoft.Extensions.Testing.Abstractions;
using NSpec.Domain;
using System.IO;

namespace NSpec.Api.Shared
{
    public class DebugInfoProvider
    {
        public DebugInfoProvider(string testAssemblyPath)
        {
            string pdbFilePath = GetPdbFilePath(testAssemblyPath);

            if (File.Exists(pdbFilePath))
            {
                sourceInfoProvider = new SourceInformationProvider(pdbFilePath);
            }
        }

        public SourceInformation GetSourceInfo(ExampleBase example)
        {
            if (sourceInfoProvider == null || example.Pending)
            {
                return emptyInfo;
            }

            var methodInfo = example.BodyMethodInfo;

            var sourceInfo = sourceInfoProvider.GetSourceInformation(methodInfo);

            return sourceInfo;
        }

        readonly SourceInformationProvider sourceInfoProvider;

        readonly SourceInformation emptyInfo = new SourceInformation(null, 0);

        static string GetPdbFilePath(string assemblyPath)
        {
            string assemblyDirectoryPath = Path.GetDirectoryName(assemblyPath);

            string assemblyFileName = Path.GetFileNameWithoutExtension(assemblyPath);

            string pdbFileName = assemblyFileName + FileNameSuffixes.DotNet.ProgramDatabase;

            string pdbFilePath = Path.Combine(assemblyDirectoryPath, pdbFileName);

            return pdbFilePath;
        }
    }
}
