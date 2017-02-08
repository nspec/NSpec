using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    public class RunnerInvocation
    {
        public ContextCollection Run()
        {
            var selector = new ContextSelector();

            selector.Select(this.dll, Tags);

            if (selector.Contexts.AnyTaggedWithFocus())
            {
                selector.Select(this.dll, Domain.Tags.Focus);
            }

            var contexts = selector.Contexts;

            var tagsFilter = selector.TagsFilter;

            var runner = new ContextRunner(tagsFilter, Formatter, failFast);

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
        string dll;
        bool failFast;
    }
}