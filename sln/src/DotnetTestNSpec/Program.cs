using System;

namespace DotNetTestNSpec
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var consoleRunner = new ConsoleRunner();

            try
            {
                consoleRunner.Run(args);

                return ReturnCodes.Ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return ReturnCodes.Error;
            }
        }

        public static class ReturnCodes
        {
            public const int Ok = 0;
            public const int Error = -1;
        }
    }
}
