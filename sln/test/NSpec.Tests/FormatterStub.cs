using NSpec.Domain;
using NSpec.Domain.Formatters;
using System.Collections.Generic;

namespace NSpec.Tests
{
    public class FormatterStub : IFormatter, ILiveFormatter
    {
        public List<WrittenContext> WrittenContexts;
        public List<WrittenExample> WrittenExamples;

        public FormatterStub()
        {
            WrittenContexts = new List<WrittenContext>();
            WrittenExamples = new List<WrittenExample>();
        }

        public void Write(ContextCollection contexts)
        {
        }

        public IDictionary<string, string> Options { get; set; }

        public void Write(Context context)
        {
            WrittenContexts.Add(new WrittenContext(context));
        }

        public void Write(ExampleBase example, int level)
        {
            WrittenExamples.Add(new WrittenExample(example));
        }
    }
}
