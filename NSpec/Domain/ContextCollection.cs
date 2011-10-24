using System.Collections.Generic;
using System.Linq;

namespace NSpec.Domain
{
    public class ContextCollection : List<Context>
    {
        public ContextCollection(IEnumerable<Context> contexts) :base(contexts){}

        public ContextCollection(){}

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
            this.Do(c => c.Run());
        }

        /// <summary>Removes contexts that do not contain any executed descendant examples</summary>
        public void TrimSkippedContexts()
        {
            this.Do( c => c.TrimSkippedDescendants() );
            this.RemoveAll( c => !c.HasDescendantExamplesExecuted() );
        }

        public IEnumerable<Context> AllContexts()
        {
            return this.SelectMany(c => c.AllContexts());
        }
    }
}