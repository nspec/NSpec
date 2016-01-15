using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    [Serializable]
    public class RunnerInvocation
    {
        public ContextCollection Run()
        {
            var reflector = new Reflector(this.dll);

            var finder = new SpecFinder(reflector);

            var tagsFilter = new Tags().Parse(Tags);

            var builder = new ContextBuilder(finder, tagsFilter, new DefaultConventions());

            var runner = new ContextRunner(tagsFilter, Formatter, failFast);

            var contexts = builder.Contexts().Build();

            if(contexts.AnyTaggedWithFocus())
            {
                tagsFilter = new Tags().Parse(NSpec.Domain.Tags.Focus);

                builder = new ContextBuilder(finder, tagsFilter, new DefaultConventions());

                runner = new ContextRunner(tagsFilter, Formatter, failFast);

                contexts = builder.Contexts().Build();
            }

            return runner.Run(contexts);
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
        public bool inDomain;  // TODO it should be removed completely
        string dll;
        bool failFast;
    }
}