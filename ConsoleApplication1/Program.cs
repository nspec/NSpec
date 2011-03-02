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
                    //new SpecFinder(@"C:\Users\Amir\NSpec\SampleSpecs\bin\Debug\SampleSpecs.dll").Run("when_inherting_from_some_shared_spec");
                    new SpecFinder(@"D:\Development\GameTrader\GameTrader.UnitTests\bin\Debug\GameTrader.UnitTests.dll").Run("describe_AuthenticationController");
            }
            catch (Exception e)
            {
                //hopefully this is handled before here, but if not, this is better than crashing the runner
                Console.WriteLine(e);
            }
            //Console.ReadLine();
        }
    }
}
