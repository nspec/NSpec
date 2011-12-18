using System;
using System.Linq;
using NSpec.Domain;
using NSpec;
using NSpecSpecs.describe_RunningSpecs;
using System.Collections.Generic;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
    public class when_running_specs
    {
        protected void Run(bool failFast)
        {
            contextCollection.Run(failFast);

            if (builder.tagsFilter.HasTagFilters()) contextCollection.TrimSkippedContexts();
        }

        protected void Run(Type type, string tags = null, bool failFast = false)
        {
            Run(new[] { type }, tags, failFast);
        }

        protected void Run(Type[] types, string tags = null, bool failFast = false)
        {

            
            Build(types, tags);
            Run(failFast);
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
                .Select(c => c as ClassContext)
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

        protected IEnumerable<Example> AllExamples()
        {
            return contextCollection.SelectMany(s => s.AllExamples());
        }

        protected Example TheExample(string name)
        {
            var theExample = contextCollection
                .SelectMany(rootContext => rootContext.AllContexts())
                .SelectMany(contexts => contexts.AllExamples().Where(example => example.Spec == name)).FirstOrDefault();

            if (theExample == null) Assert.Fail("Did not find example named: " + name);

            theExample.Spec.should_be(name);

            return theExample;
        }

        protected ContextBuilder builder;
        protected ContextCollection contextCollection;
        protected ClassContext classContext;
        protected Context methodContext;
        protected ContextCollection contexts;
        protected FormatterStub formatter;
    }
}