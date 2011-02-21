using System;
using NSpec;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length>1)
                new SpecFinder(args[0]).Run(args[1]);
            else
                new SpecFinder(args[0]).Run();
        }
    }
}
