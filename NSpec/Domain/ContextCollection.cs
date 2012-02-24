using System;
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
            return Examples().Where(e => e.Exception != null);
        }

        public IEnumerable<Example> Pendings()
        {
            return Examples().Where(e => e.Pending);
        }

        public ContextCollection Build()
        {
            this.Do(c => c.Build());

            return this;
        }

        public void Run(bool failFast = false)
        {
            Run(new SilentLiveFormatter(), failFast);
        }

        public void Run(ILiveFormatter formatter, bool failFast)
        {
            this.Do(c => c.Run(formatter, failFast: failFast));
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

        public Context Find(string name)
        {
            return AllContexts().FirstOrDefault(c => c.Name == name);
        }

        public Example FindExample(string name)
        {
            return Examples().FirstOrDefault(e => e.Spec == name);
        }
    }
}