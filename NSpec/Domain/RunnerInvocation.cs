using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    [Serializable]
    public class RunnerInvocation
    {
        public string Tags;
        public IFormatter Console;
        public string Dll;

        public RunnerInvocation(string dll, string tags, IFormatter console)
        {
            Tags = tags;
            Console = console;
            Dll = dll;
        }

        public ContextRunner Runner()
        {
            var finder = new SpecFinder(Dll, new Reflector());

            var tagsFilter = new Tags().Parse(Tags);

            var builder = new ContextBuilder(finder, tagsFilter, new DefaultConventions());

            return new ContextRunner(builder, Console);
        }
    }
}