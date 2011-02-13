using System;
using NSpec;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            new Finder(@"C:\Users\matt\Documents\visual studio 2010\Projects\NSpec\SampleSpecs\bin\Debug\SampleSpecs.dll").Run();
            Console.Read();
        }
    }
}
