using System;
using System.Linq;
using NSpec.Domain;
using NSpec;
using NSpecSpecs.describe_RunningSpecs;
using System.Collections.Generic;
using NUnit.Framework;
using System.Reflection;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    public class when_running_specs
    {
        [SetUp]
        public void InitializeRunnerInvocation()
        {
            formatter = new FormatterStub();
        }

        protected when_running_specs Init(Type type, string tags = null, bool failFast = false)
        {
            return Init(new Type[] { type }, tags, failFast);
        }

        protected when_running_specs Init(Type[] types, string tags = null, bool failFast = false)
        {
            this.types = types;

            invocation = new RunnerInvocation(tags ?? types.First().Name, formatter, new SpecFinder(types), failFast);

            builder = invocation.Builder();

            contextCollection = builder.Contexts();

            contextCollection.Build();

            classContext = contextCollection
                .AllContexts()
                .Select(c => c as ClassContext)
                .FirstOrDefault(c => types.Contains(c.type));

            methodContext = contextCollection.AllContexts().FirstOrDefault(c => c is MethodContext);

            return this;
        }

        public void Run()
        {
            invocation.Runner().Run(contextCollection);
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
        protected bool failFast;
        protected RunnerInvocation invocation;
        protected Context methodContext;
        protected ContextCollection contexts;
        protected FormatterStub formatter;
        protected Type[] types;
    }
}