using System;
using System.Reflection;
using NSpec;
using NSpec.Domain;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsage();
                return;
            }
            try
            {
                var classFilter = args.Length > 1 ? args[1] : "";

                var finder = new SpecFinder(args[0], new Reflector(), classFilter);

                var builder = new ContextBuilder(finder, new DefaultConvention());

                new ContextRunner(builder).Run();
            }
            catch (Exception e)
            {
                //hopefully this is handled before here, but if not, this is better than crashing the runner
                Console.WriteLine(e);
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("VERSION: {0}".With(Assembly.GetExecutingAssembly().GetName().Version));
            Console.WriteLine();
            Console.WriteLine("Example usage:");
            Console.WriteLine();
            Console.WriteLine("nspecrunner path_to_spec_dll [regex pattern]");
            Console.WriteLine();
            Console.WriteLine("The second parameter is optional. If supplied, only the classes that match the regex will be run.  The full class name including namespace is considered. Otherwise all spec classes in the dll will be run.");
        }
    }
}
