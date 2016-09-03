using DotnetTestNSpec.Compatibility;
using System;
using System.Linq;
using System.Reflection;

namespace DotnetTestNSpec
{
    public class ConsoleRunner
    {
        public int Run(string[] args)
        {
            var testRunnerAssembly = typeof(Program).GetTypeInfo().Assembly;

            Console.WriteLine(testRunnerAssembly.GetPrintInfo());

            var dotNetTestArgs = args.TakeWhile(arg => arg != "--");
            var nSpecArgs = args.Skip(dotNetTestArgs.Count() + 1);

            /*
            var nSpecAssembly = ...;
            Console.WriteLine(nSpecAssembly.GetPrintInfo());
            */

            return ReturnCodes.Ok;
        }
    }
}
