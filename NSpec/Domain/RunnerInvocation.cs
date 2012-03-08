using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    [Serializable]
    public class RunnerInvocation
    {
        public ContextCollection Run()
        {
            var finder = new SpecFinder(this.dll, new Reflector());

            var builder = new ContextBuilder(finder, new Tags().Parse(Tags), new DefaultConventions());

            var runner = new ContextRunner(builder, Formatter, failFast);

            return runner.Run(builder.Contexts().Build());
        }

        public RunnerInvocation(string dll, string tags)
            : this(dll, tags, false) {}

        public RunnerInvocation(string dll, string tags, bool failFast)
            : this(dll, tags, new ConsoleFormatter(), failFast) {}

        public RunnerInvocation(string dll, string tags, IFormatter formatter, bool failFast)
        {
            this.dll = dll;
            this.failFast = failFast;
            Tags = tags;
            Formatter = formatter;
        }

        public string Tags;
        public IFormatter Formatter;
        public bool inDomain;
        string dll;
        bool failFast;
    }
}