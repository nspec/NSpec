using System;
using System.Reflection;
using System.Threading;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;

namespace NSpecRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs commandLineArgs = CommandLineArgs.Parse( args );

            try
            {
                Console.WriteLine( commandLineArgs.SpecDll);
                var domain = new NSpecDomain(commandLineArgs.SpecDll + ".config");

                domain.Run(commandLineArgs.SpecDll, commandLineArgs.ClassFilter, (dll, filter) =>
                {
                    var finder = new SpecFinder(dll, new Reflector(), filter);

                    var builder = new ContextBuilder(finder, new DefaultConventions());

					Thread.Sleep( commandLineArgs.DebugWaitTimeInSeconds * 1000 );

                    var runner = new ContextRunner(builder, commandLineArgs.OutputFormatter);
                    runner.Run();
                });
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

            commandLineArgs.SpecDll = args[0];

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
                    commandLineArgs.TemplateFileName = args[++i];
                    commandLineArgs.OutputFileName = args[++i];
                    commandLineArgs.OutputFormatter = new TiddlyWikiFormatter( commandLineArgs.TemplateFileName,
                                                                               commandLineArgs.OutputFileName );
                    continue;
                }
                if( args[i] == "--xml" )
                {
                    commandLineArgs.OutputFormatter = new XmlFormatter();
                    continue;
                }
                if( args[i] == "--html" )
                {
                    commandLineArgs.OutputFormatter = new HtmlFormatter();
                    continue;
                }
                if( args[i] == "--debug" )
                {
                    int waitTime = 0;
                    Int32.TryParse( args[++i], out waitTime );
                    commandLineArgs.DebugWaitTimeInSeconds = waitTime;
                    continue;
                }
            }

            return commandLineArgs;
        }

        static void PrintUsage()
        {
            Console.WriteLine("VERSION: {0}".With(Assembly.GetExecutingAssembly().GetName().Version));
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
            Console.WriteLine( " --html                                   The output will be in html format" );
            Console.WriteLine( " --debug <wait time in seconds>           Delays the execution of the specs so you can attach a debugger to the runner" );
            System.Environment.Exit( 1 );
        }

        public string SpecDll { get; set; }
        public string ClassFilter { get; set; }
        public IFormatter OutputFormatter { get; set; }
        public string TemplateFileName { get; set; }
        public string OutputFileName { get; set; }
        public int DebugWaitTimeInSeconds { get; set; }

        private CommandLineArgs()
        {
            this.SpecDll = "";
            this.ClassFilter = "";
            this.OutputFormatter = new ConsoleFormatter();
            this.OutputFileName = "";
            this.DebugWaitTimeInSeconds = 0;
        }
    }
}
