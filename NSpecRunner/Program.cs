using System;
using NSpec;
using NSpec.Domain;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var classFilter = args.Length > 1 ? args[1] : "";

                //var finder = new SpecFinder(args[0], new Reflector(), classFilter);

                //var finder = new SpecFinder(@"C:\Development\GameTrader\GameTrader.Specs\bin\Debug\GameTrader.Specs.dll", new Reflector(), "desribe_AuthenticationController");
                var finder = new SpecFinder(@"C:\users\amir\nspec\samplespecs\bin\debug\samplespecs.dll", new Reflector(), "describe_car");

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
    }
}
