using DotNetTestNSpec.Compatibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DotNetTestNSpec
{
    public class ConsoleRunner
    {
        public int Run(string[] args)
        {
            var testRunnerAssembly = typeof(Program).GetTypeInfo().Assembly;

            Console.WriteLine(testRunnerAssembly.GetPrintInfo());

            var argumentParser = new ArgumentParser();

            CommandLineOptions options = argumentParser.Parse(args);

            Console.WriteLine(options);

            if (options.Project == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }

            var nspecLibraryAssembly = GetNSpecLibraryAssembly(options.Project);

            Console.WriteLine(nspecLibraryAssembly.GetPrintInfo());

            var controllerProxy = new ControllerProxy(nspecLibraryAssembly);

            // TODO extract and pass all controller parameter: tags, formatterClassName, formatterOptions
            int nrOfFailures = controllerProxy.Run(options.Project, "", "", new Dictionary<string, string>(), false);

            return nrOfFailures;
        }

        static Assembly GetNSpecLibraryAssembly(string testAssemblyPath)
        {
            string outputAssemblyDirectory = Path.GetDirectoryName(testAssemblyPath);

            string nspecLibraryAssemblyPath = Path.Combine(outputAssemblyDirectory, nspecFileName);

            try
            {
                var nspecLibraryAssembly = AssemblyUtils.LoadFromPath(nspecLibraryAssemblyPath);

                return nspecLibraryAssembly;
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(
                    $"Could not load referenced NSpec library assembly at '{nspecLibraryAssemblyPath}'",
                    ex);
            }
        }

        const string nspecFileName = "NSpec.dll";
    }
}
