namespace DotNetTestNSpec
{
    public class CommandLineOptions
    {
        public string Project { get; set; }

        public int? ParentProcessId { get; set; }

        public int? Port { get; set; }

        public string[] NSpecArgs { get; set; }

        public string[] UnknownArgs { get; set; }

        public override string ToString()
        {
            return EnumerableUtils.ToObjectString(new string[]
            {
                $"{nameof(Project)}: {Project}",
                $"{nameof(ParentProcessId)}: {ParentProcessId}",
                $"{nameof(Port)}: {Port}",
                $"{nameof(NSpecArgs)}: {EnumerableUtils.ToArrayString(NSpecArgs)}",
                $"{nameof(UnknownArgs)}: {EnumerableUtils.ToArrayString(UnknownArgs)}",
            }, true);
        }
    }
}
