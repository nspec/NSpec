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
                    outputFormatter = new TiddlyWikiFormatter( commandLineArgs.TemplateFileName, commandLineArgs.OutputFileName );
                }
                else if( commandLineArgs.XmlOutput )
                {
                    outputFormatter = new XmlFormatter();
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

            for( int i = 1; i < args.Length; i++ )
            {
                if( args[i] == "--classFilter" ||
                    args[i] == "-cf" )
                {
                    commandLineArgs.ClassFilter = args[++i];
                    continue;
                }
                if( args[i] == "--tiddlyWiki" )
                {
                    commandLineArgs.TiddlyWikiOutput = true;
                    commandLineArgs.TemplateFileName = args[++i];
                    commandLineArgs.OutputFileName = args[++i];
                    continue;
                }
                if( args[i] == "--xml" )
                {
                    commandLineArgs.XmlOutput = true;
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
            Console.WriteLine( "Options:");
            Console.WriteLine( " -cf, --classFilter <regex pattern>       Only the classes that match the" );
            Console.WriteLine( "                                          regex will be run.  The full class name" );
            Console.WriteLine( "                                          including namespace is considered." );
            Console.WriteLine( " --tiddlyWiki <template> <destination>    Redirects the output to the file name" );
            Console.WriteLine( "                                          provided in a TiddyWiki format using" );
            Console.WriteLine( "                                          the template provided" );
            Console.WriteLine( " --xml                                    The output will be in xml format" );
            System.Environment.Exit( 1 );
        }

        public string ClassFilter { get; set; }
        public bool TiddlyWikiOutput { get; set; }
        public bool XmlOutput { get; set; }
        public string TemplateFileName { get; set; }
        public string OutputFileName { get; set; }

        private CommandLineArgs()
        {
            this.ClassFilter = "";
            this.TiddlyWikiOutput = false;
            this.XmlOutput = false;
            this.OutputFileName = "";
        }
    }
}
