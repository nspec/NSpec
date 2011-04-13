using System;
using System.Net.Mime;
using System.Reflection;
using NSpec;
using NSpec.Domain;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length==0)
            {
                ShowUsage();
                return;
            }
            try
            {
                var classFilter = args.Length > 1 ? args[1] : "";

                var finder = new SpecFinder(args[0], new Reflector(), classFilter);

                //var finder = new SpecFinder(@"C:\Development\GameTrader\GameTrader.Specs\bin\Debug\GameTrader.Specs.dll", new Reflector(), "desribe_AuthenticationController");
                //var finder = new SpecFinder(@"C:\users\amir\nspec\samplespecs\bin\debug\samplespecs.dll", new Reflector(), "class_level_before");

                var builder = new ContextBuilder(finder);

                new ContextRunner(builder).Run();

                //new ContextRunner(new ContextBuilder(new SpecFinder(@"C:\Development\GameTrader\GameTrader.Specs\bin\Debug\GameTrader.Specs.dll", new Reflector()))).Run();
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
            Console.WriteLine("nspecrunner path_to_spec_dll [name_of_spec_class_to_run]");
            Console.WriteLine();
            Console.WriteLine("The second parameter is optional. If supplied only the single class will be run. Otherwise all spec classes in the dll will be run.");
        }
    }
}
