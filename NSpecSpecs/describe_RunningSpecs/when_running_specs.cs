using System;
using System.Linq;
using NSpec.Domain;
using NSpec;

namespace NSpecSpecs.WhenRunningSpecs
{
    public class when_running_specs
    {
        protected void Run(Type type)
        {
            var finder = new SpecFinder(new[] { type });

            var builder = new ContextBuilder(finder, new DefaultConventions());

            var contexts = builder.Contexts();

            contexts.Build();

            contexts.Run();

            classContext = contexts
                .AllContexts()
                .Select(c => c as ClassContext)
                .First(c => c.type == type);

            methodContext = contexts.AllContexts().First(c => c is MethodContext);
        }

        protected Context TheContext( string name )
        {
            var theContext = classContext.AllContexts()
                .SelectMany( contexts => contexts.AllContexts().Where( context => context.Name == name ) )
                .First();

            theContext.Name.should_be( name );

            return theContext;
        }

        protected Example TheExample( string name )
        {
            var theExample = classContext.AllContexts()
                .SelectMany( contexts => contexts.AllExamples().Where( example => example.Spec == name ) )
                .First();

            theExample.Spec.should_be( name );
    
            return theExample;
        }

        protected ClassContext classContext;
        protected Context methodContext;
    }
}