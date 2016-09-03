using DotnetTestNSpec.Compatibility;
using System;
using System.Reflection;

namespace DotnetTestNSpec
{
    public class ConsoleRunner
    {
        public int Run(string[] args)
        {
            var testRunnerAssembly = typeof(Program).GetTypeInfo().Assembly;

            Console.WriteLine(testRunnerAssembly.GetPrintInfo());

            /*
            var nSpecAssembly = ...;
            Console.WriteLine(nSpecAssembly.GetPrintInfo());
            */

            return ReturnCodes.Ok;
        }
    }
}
