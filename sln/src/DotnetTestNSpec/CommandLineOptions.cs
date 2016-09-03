namespace DotnetTestNSpec
{
    public class CommandLineOptions
    {
        public string Project { get; set; }

        public int? ParentProcessId { get; set; }

        public int? Port { get; set; }

        public string[] NSpecArgs { get; set; }

        public string[] UnknownArgs { get; set; }
    }
}
