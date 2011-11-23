using System.Collections.Generic;
using System.Linq;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    public class ContextCollection : List<Context>
    {
        public ContextCollection(IEnumerable<Context> contexts) : base(contexts) { }

        public ContextCollection() { }

        public IEnumerable<Example> Examples()
        {
            return this.SelectMany(c => c.AllExamples());
        }

        public IEnumerable<Example> Failures()
        {
            return Examples().Where(e => e.ExampleLevelException != null);
        }

        public IEnumerable<Example> Pendings()
        {
            return Examples().Where(e => e.Pending);
        }

        public void Build()
        {
            this.Do(c => c.Build());
        }

        public void Run()
        {
            Run(new ConsoleFormatter());
        }

        public void Run(ILiveFormatter formatter)
        {
            this.Do(c => c.Run(formatter));
        }

        public void TrimSkippedContexts()
        {
            this.Do(c => c.TrimSkippedDescendants());

            this.RemoveAll(c => !c.HasAnyExecutedExample());
        }

        public IEnumerable<Context> AllContexts()
        {
            return this.SelectMany(c => c.AllContexts());
        }
    }
}