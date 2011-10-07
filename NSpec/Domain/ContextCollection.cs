using System;
using System.Collections.Generic;
using System.Linq;
using TestDriven.Framework;

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
            return Examples().Where(e => e.Exception != null);
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

        public IEnumerable<Context> AllContexts()
        {
            return this.SelectMany(c => c.AllContexts());
        }

        public TestRunState Result()
        {
            if (Examples().Count() == 0) return TestRunState.NoTests;

            if (Failures().Count() == 0) return TestRunState.Success;

            return TestRunState.Failure;
        }
    }
}