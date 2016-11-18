using System.Collections.Generic;

namespace DotNetTestNSpec
{
    public class NSpecCommandLineOptions
    {
        public string ClassName { get; set; }

        public string Tags { get; set; }

        public bool FailFast { get; set; }

        public string FormatterName { get; set; }

        public Dictionary<string, string> FormatterOptions { get; set; }

        public string[] UnknownArgs { get; set; }

        public override string ToString()
        {
            return EnumerableUtils.ToObjectString(new string[]
            {
                $"{nameof(ClassName)}: {ClassName}",
                $"{nameof(Tags)}: {Tags}",
                $"{nameof(FailFast)}: {FailFast}",
                $"{nameof(FormatterName)}: {FormatterName}",
                $"{nameof(FormatterOptions)}: {DictionaryUtils.ToArrayString(FormatterOptions)}",
                $"{nameof(UnknownArgs)}: {EnumerableUtils.ToArrayString(UnknownArgs)}",
            }, true);
        }
    }
}
