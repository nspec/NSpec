using System;
using System.Linq;
using NSpec.Domain;
using NSpec;
using NSpecSpecs.describe_RunningSpecs;

namespace NSpecSpecs.WhenRunningSpecs
{
    public class when_running_specs
    {
        protected void Run()
        {
            contextCollection.Run();

            if (builder.tagsFilter.HasTagFilters()) contextCollection.TrimSkippedContexts();
        }

        protected void Run(Type type, string tags = null)
        {
            Run(new[] { type }, tags);
        }

        protected void Run(Type[] types, string tags = null)
        {
            Build(types, tags);
            Run();
        }

        protected void Build(Type type, string tags = null)
        {
            Build(new[] { type }, tags);
        }

        protected void Build(Type[] types, string tags = null)
        {
            var finder = new SpecFinder(types); 

            builder = new ContextBuilder(finder, new Tags().Parse(tags), new DefaultConventions());

            contextCollection = builder.Contexts();

            contextCollection.Build();

            classContext = contextCollection
                .AllContexts()
                .Where(c => c is ClassContext)
                .Cast<ClassContext>()
                .FirstOrDefault(c => types.Contains(c.type));

            methodContext = contextCollection.AllContexts().FirstOrDefault(c => c is MethodContext);
        }

        protected Context TheContext(string name)
        {
            var theContext = contextCollection
                .SelectMany(rootContext => rootContext.AllContexts())
                .SelectMany(contexts => contexts.AllContexts().Where(context => context.Name == name)).First();

            theContext.Name.should_be(name);

            return theContext;
        }

        protected Example TheExample(string name)
        {
            var theExample = contextCollection
                .SelectMany(rootContext => rootContext.AllContexts())
                .SelectMany(contexts => contexts.AllExamples().Where(example => example.Spec == name)).First();

            theExample.Spec.should_be(name);

            return theExample;
        }

        protected ContextBuilder builder;
        protected ContextCollection contextCollection;
        protected ClassContext classContext;
        protected Context methodContext;
        protected ContextCollection contexts;
    }
}