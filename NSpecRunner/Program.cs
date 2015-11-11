using System;
using System.Linq;
using System.Reflection;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;

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

                var formatter = FindFormatter(formatterClassName);

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

                var specDLL = args[0];

                var invocation = new RunnerInvocation(specDLL, argsTags, formatter, failFast);

                var domain = new NSpecDomain(specDLL + ".config");

                var failures = domain.Run(invocation, i => i.Run().Failures().Count(), specDLL);

                if (failures > 0) Environment.Exit(1);
            }
            catch (Exception e)
            {
                //hopefully this is handled before here, but if not, this is better than crashing the runner
                Console.WriteLine(e);
                Environment.Exit(1);
            }
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

        /// <summary>
        /// Find an implementation of IFormatter with the given class name
        /// </summary>
        /// <param name="formatterClassName"></param>
        /// <returns></returns>
        private static IFormatter FindFormatter(string formatterClassName)
        {
            // Default formatter is the standard console formatter
            if (string.IsNullOrEmpty(formatterClassName))
            {
                return new ConsoleFormatter();
            }

            Assembly nspecAssembly = typeof(IFormatter).Assembly;

            // Look for a class that implements IFormatter with the provided name
            var formatterType = nspecAssembly.GetTypes().FirstOrDefault(type =>
                (type.Name.ToLowerInvariant() == formatterClassName)
                && typeof(IFormatter).IsAssignableFrom(type) );

            if (formatterType != null)
            {
                return (IFormatter)Activator.CreateInstance(formatterType);
            }
            else
            {
                throw new TypeLoadException("Could not find formatter type " + formatterClassName);

            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("VERSION: {0}".With(Assembly.GetExecutingAssembly().GetName().Version));
            Console.WriteLine();
            Console.WriteLine("Example usage:");
            Console.WriteLine();
            Console.WriteLine("nspecrunner path_to_spec_dll [classname]");
            Console.WriteLine();
            Console.WriteLine("The second parameter is optional. If supplied, only that specific test class will run.  Otherwise all spec classes in the dll will be run.");
            Console.WriteLine();
            Console.WriteLine("nspecrunner path_to_spec_dll --tag classname");
            Console.WriteLine();
            Console.WriteLine("The command above is equivalent to specifing the second parameter in: nspecrunner path_to_spec_dll [classname]");
            Console.WriteLine();
            Console.WriteLine("Example usage (tagging):");
            Console.WriteLine();
            Console.WriteLine("nspecrunner path_to_spec_dll --tag tag1,tag2,tag3");
            Console.WriteLine();
            Console.WriteLine("This will run all tests under tags specified.  A test class's name is automatically considered in tagging.");
            Console.WriteLine();
            Console.WriteLine("Example usage (failfast):");
            Console.WriteLine();
            Console.WriteLine("nspecrunner path_to_spec_dll [classname] --failfast");
            Console.WriteLine();
            Console.WriteLine("Adding --failfast to any of the commands above will stop execution immediately when a failure is encountered.");
            Console.WriteLine();
            Console.WriteLine("nspecrunner path_to_spec_dll [classname] --formatter=formatterClass");
            Console.WriteLine();
            Console.WriteLine("You can optionally specify a formatter for the output by providing the class name of the desired formatter.");

        }
    }
}
