using NSpec.Domain;
using NSpec.Domain.Formatters;
using System.Collections.Generic;

namespace NSpec.Tests
{
    public class FormatterStub : IFormatter, ILiveFormatter
    {
        public List<Context> WrittenContexts;
        public List<ExampleBase> WrittenExamples;

        public FormatterStub()
        {
            WrittenContexts = new List<Context>();
            WrittenExamples = new List<ExampleBase>();
        }

        public void Write(ContextCollection contexts)
        {
        }

        public IDictionary<string, string> Options { get; set; }


        public void Write(Context context)
        {
            WrittenContexts.Add(context);
        }

        public void Write(ExampleBase example, int level)
        {
            WrittenExamples.Add(example);
        }
    }
}
