using System;
using System.Reflection;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs commandLineArgs = CommandLineArgs.Parse( args );

            try
            {
                var finder = new SpecFinder(args[0], new Reflector(), commandLineArgs.ClassFilter);

                var builder = new ContextBuilder(finder, new DefaultConventions());

                IFormatter outputFormatter = new ConsoleFormatter();
                if( commandLineArgs.TiddlyWikiOutput )
                {
                    outputFormatter = new TiddlyWikiFormatter( commandLineArgs.OutputFileName );
                }

                new ContextRunner(builder, outputFormatter).Run();
            }
            catch (Exception e)
            {
                //hopefully this is handled before here, but if not, this is better than crashing the runner
                Console.WriteLine(e);
            }
        }
    }


    class CommandLineArgs
    {
        public static CommandLineArgs Parse( string[] args )
        {
            CommandLineArgs commandLineArgs = new CommandLineArgs();

            if( args.Length == 0 )
            {
                PrintUsage();
            }

            for( int i = 0; i < args.Length; i++ )
            {
                if( args[i] == "-classFilter" )
                {
                    commandLineArgs.ClassFilter = args[++i];
                    continue;
                }
                if( args[i] == "-tiddlyWikiOutput" )
                {
                    commandLineArgs.TiddlyWikiOutput = true;
                    commandLineArgs.OutputFileName = args[++i];
                    continue;
                }
            }

            return commandLineArgs;
        }

        static void PrintUsage()
        {
            Console.WriteLine( "VERSION: {0}".With( Assembly.GetExecutingAssembly().GetName().Version ) );
            Console.WriteLine();
            Console.WriteLine( "Usage: NSpecRunner path_to_spec_dll [options]" );
            Console.WriteLine();
            Console.WriteLine( "  -classFilter [regex pattern]" );
            Console.WriteLine( "  -tiddlyWikiOutput [output filename]" );
            System.Environment.Exit( 1 );
        }

        public string ClassFilter { get; set; }
        public bool TiddlyWikiOutput { get; set; }
        public string OutputFileName { get; set; }

        private CommandLineArgs()
        {
            this.ClassFilter = "";
            this.TiddlyWikiOutput = false;
            this.OutputFileName = "";
        }
    }
}
