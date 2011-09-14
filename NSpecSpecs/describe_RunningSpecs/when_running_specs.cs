using System;
using System.Linq;
using NSpec.Domain;
using NUnit.Framework;
using NSpec;
using Rhino.Mocks;

namespace NSpecSpecs.WhenRunningSpecs
{
    public class when_running_specs
    {
        [SetUp]
        public void setup_base()
        {
            convention = new DefaultConventions();

            convention.Initialize();
        }

        protected void Run(params Type[] types)
        {
            var finder = MockRepository.GenerateMock<ISpecFinder>();

            finder.Stub(f => f.SpecClasses()).Return(types);

            var builder = new ContextBuilder(finder, new DefaultConventions());

            var contexts = builder.Contexts();

            contexts.Build();

            contexts.Run();

            classContext = contexts.First() as ClassContext;

            methodContext = contexts.AllContexts().First(c => c is MethodContext);
        }

        protected ClassContext classContext;
        private DefaultConventions convention;
        protected Context methodContext;
    }
}