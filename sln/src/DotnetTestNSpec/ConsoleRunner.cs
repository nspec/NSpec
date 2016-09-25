using DotNetTestNSpec.Compatibility;
using System;
using System.Reflection;

namespace DotNetTestNSpec
{
    public class ConsoleRunner
    {
        public void Run(string[] args)
        {
            var testRunnerAssembly = typeof(Program).GetTypeInfo().Assembly;

            Console.WriteLine(testRunnerAssembly.GetPrintInfo());

            var argumentParser = new ArgumentParser();

            CommandLineOptions options = argumentParser.Parse(args);

            Console.WriteLine(options);

            /*
            var nSpecAssembly = ...;
            Console.WriteLine(nSpecAssembly.GetPrintInfo());
            */
        }
    }
}
