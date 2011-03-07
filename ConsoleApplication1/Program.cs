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
                    new SpecFinder(args[0],new Reflector()).Run(args[1]);
                else if (args.Length == 1)
                    new SpecFinder(args[0], new Reflector()).Run();
                else
                    new SpecFinder(@"C:\Development\GameTrader\GameTrader.UnitTests\bin\Debug\GameTrader.UnitTests.dll",new Reflector()).Run("describe_UserController");
            }
            catch (Exception e)
            {
                //hopefully this is handled before here, but if not, this is better than crashing the runner
                Console.WriteLine(e);
            }
        }
    }
}
