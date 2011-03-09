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
                var finder = new SpecFinder(args[0], new Reflector());

                if (args.Length > 1)
                    new ContextBuilder(finder).Run(args[1]);
                else if (args.Length == 1)
                    new ContextBuilder(finder).Run();
                else
                    new ContextBuilder(new SpecFinder(@"C:\Development\GameTrader\GameTrader.UnitTests\bin\Debug\GameTrader.UnitTests.dll",new Reflector())).Run("describe_AuthenticationController");
            }
            catch (Exception e)
            {
                //hopefully this is handled before here, but if not, this is better than crashing the runner
                Console.WriteLine(e);
            }
        }
    }
}
