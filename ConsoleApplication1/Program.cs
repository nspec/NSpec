using System;
using NSpec;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 1)
                    new SpecFinder(args[0]).Run(args[1]);
                else if (args.Length == 1)
                    new SpecFinder(args[0]).Run();
                else
                    new SpecFinder(@"C:\Users\Amir\NSpec\NSpecSpec\bin\Debug\nspecspec.dll").Run("nspec_bug");
            }
            catch (Exception e)
            {
                //hopefully this is handled before here, but if not, this is better than crashing the runner
                Console.WriteLine(e);
            }
        }
    }
}
