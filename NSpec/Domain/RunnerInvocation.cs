using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    [Serializable]
    public class RunnerInvocation
    {
        private ContextRunner contextRunner;
        private ContextBuilder contextBuilder;
        public ISpecFinder SpecFinder;
        public string Tags;
        public IFormatter Console;


        public RunnerInvocation(string tags, IFormatter console, ISpecFinder specFinder, bool failFast)
        {
            Tags = tags;
            Console = console;
            SpecFinder = specFinder;
            contextBuilder = new ContextBuilder(SpecFinder, TagsFilter(), new DefaultConventions());
            contextRunner = new ContextRunner(Builder(), Console, failFast);
        }

        public ContextCollection Run()
        {
            return Runner().Run(Builder().Contexts().Build());
        }

        public ContextRunner Runner()
        {
            return contextRunner;
        }

        public ContextBuilder Builder()
        {
            return contextBuilder;
        }

        public Tags TagsFilter()
        {
            return new Tags().Parse(Tags);
        }
    }
}