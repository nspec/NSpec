using System;
using System.Linq;
using NSpec.Domain;
using NSpec;

namespace NSpecSpecs.WhenRunningSpecs
{
    public class when_running_specs
    {
        protected void Run(Type type, Tags tagsFilter = null)
        {
            var finder = new SpecFinder(new[] { type });

            var builder = new ContextBuilder( finder, tagsFilter, new DefaultConventions() );

            var contexts = builder.Contexts();

            contexts.Build();

            contexts.Run();

            // remove any contexts that ended with no examples (which is likely due to presence of tag filters)
            if( builder.tagsFilter != null && builder.tagsFilter.HasTagFilters() )
                contexts.TrimEmptyContexts();

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