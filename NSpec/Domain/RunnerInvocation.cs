using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    [Serializable]
    public class RunnerInvocation
    {
        public string Tags;
        public IFormatter Console;

        public RunnerInvocation(string tags, IFormatter console)
        {
            Tags = tags;
            Console = console;
        }

        public ContextRunner Runner(ISpecFinder specFinder)
        {
            var finder = specFinder;

            var tagsFilter = new Tags().Parse(Tags);

            var builder = new ContextBuilder(finder, tagsFilter, new DefaultConventions());

            return new ContextRunner(builder, Console);
        }
    }
}