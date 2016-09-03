namespace DotnetTestNSpec
{
    public class CommandLineOptions
    {
        public int? ParentProcessId { get; set; }

        public int? Port { get; set; }

        public string[] NSpecArgs { get; set; }

        public string[] UnknownArgs { get; set; }
    }
}
