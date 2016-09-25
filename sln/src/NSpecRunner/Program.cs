using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec;
using System.IO;
using NSpec.Api;

namespace NSpecRunner
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsage();
                return;
            }
            try
            {
                // extract either a class filter or a tags filter (but not both)
                var argsTags = "";

                var failFast = IsFailFast(args);
                var formatterClassName = GetFormatterClassName(args);
                var formatterOptions = GetFormatterOptions(args);

                args = RemoveOptionsAndSwitches(args);

                if (args.Length > 1)
                {
                    // see rspec and cucumber for ideas on better ways to handle tags on the command line:
                    // https://github.com/cucumber/cucumber/wiki/tags
                    // https://www.relishapp.com/rspec/rspec-core/v/2-4/docs/command-line/tag-option
                    if (args[1] == "--tag" && args.Length > 2)
                        argsTags = args[2];
                    else
                        argsTags = args[1];
                }

                var specDLL = Path.GetFullPath(args[0]);

                var controller = new Controller();

                var failures = controller.Run(specDLL, argsTags, formatterClassName, formatterOptions, failFast);

                if (failures > 0) Environment.Exit(1);
            }
            catch (Exception e)
            {
                //hopefully this is handled before here, but if not, this is better than crashing the runner
                Console.WriteLine(e);
                Environment.Exit(1);
            }
        }

        static IDictionary<string, string> GetFormatterOptions(string[] args)
        {
            var formatterOptions = args.Where(s => s.StartsWith("--formatterOptions:", StringComparison.OrdinalIgnoreCase));
            return formatterOptions.Select(s =>
            {
                var opt = s.Substring("--formatterOptions:".Length);
                var parts = opt.Split('=');
                if (parts.Length == 2)
                    return new KeyValuePair<string, string>(parts[0], parts[1]);
                else
                    return new KeyValuePair<string, string>(parts[0], parts[0]);
            }).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static string[] RemoveOptionsAndSwitches(string[] args)
        {
            return args.Where(s => !s.StartsWith("--") || s == "--tag" ).ToArray();
        }

        public static bool IsFailFast(string[] args)
        {
            return args.Any(s => s == "--failfast");
        }

        public static string GetFormatterClassName(string[] args)
        {
            string formatter = args.FirstOrDefault(s => s.StartsWith("--formatter=") );
            if (formatter != null)
            {
                return formatter.Substring("--formatter=".Length).ToLowerInvariant();
            }
            else
            {
                return null;
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("VERSION: {0}".With(typeof(Program).GetTypeInfo().Assembly.GetName().Version));
            Console.WriteLine(@"
Example usage:

.NET Framework:
nspecrunner path_to_spec_dll [classname]

.NET Core:
dotnet path_to\NSpecRunner.dll path_to_spec_dll [classname]

The `classname` parameter is optional. If supplied, only that specific test class will be run.  Otherwise all spec classes in the dll will be run.
From here one, when not specified, .NET Core syntax follows same rules as .NET Framework syntax.

nspecrunner path_to_spec_dll --tag classname

The command above is equivalent to specifing the `classname` parameter in: nspecrunner path_to_spec_dll [classname].

Example usage (tagging):

nspecrunner path_to_spec_dll --tag tag1,tag2,tag3

This will run all tests under tags specified.  A test class's name is automatically considered in tagging.

Example usage (failfast):

nspecrunner path_to_spec_dll [classname] --failfast

Adding --failfast to any of the commands above will stop execution immediately when a failure is encountered.

nspecrunner path_to_spec_dll [classname] --formatter=formatterClass

You can optionally specify a formatter for the output by providing the class name of the desired formatter.
nspecrunner path_to_spec_dll [classname] --formatter=formatterClass --formatterOptions:optName=optValue

You can optionally specify options for the formatter. These are passed to the formatter class. See formatters for supported options");
        }
    }
}
