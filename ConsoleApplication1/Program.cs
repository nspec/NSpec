using System;
using NSpec;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            new SpecFinder().Run();
            Console.Read();
        }
    }
}
