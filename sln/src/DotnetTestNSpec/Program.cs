namespace DotnetTestNSpec
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var consoleRunner = new ConsoleRunner();

            int returnCode = consoleRunner.Run(args);

            return returnCode;
        }
    }
}
