using System;
using System.Linq;
using NSpec.Domain;
using NSpec;

namespace NSpecSpecs.WhenRunningSpecs
{
    public class when_running_specs
    {
        protected void Run( Type type, Tags tagsFilter = null )
        {
            Run( new[] { type }, tagsFilter );
        }

        protected void Run(Type[] types, Tags tagsFilter = null)
        {
            var finder = new SpecFinder(types);

            var builder = new ContextBuilder( finder, tagsFilter, new DefaultConventions() );

            contextCollection = builder.Contexts();

            contextCollection.Build();

            contextCollection.Run();

            // remove any contexts that ended with no examples (which is likely due to presence of tag filters)
            if( builder.tagsFilter != null && builder.tagsFilter.HasTagFilters() )
                contextCollection.TrimEmptyContexts();

            classContext = contextCollection
                .AllContexts()
                .Select(c => c as ClassContext)
                .FirstOrDefault(c => types.Contains( c.type ) );

            methodContext = contextCollection.AllContexts().FirstOrDefault(c => c is MethodContext);
        }

        protected Context TheContext( string name )
        {
            var theContext = contextCollection
                .SelectMany( rootContext => rootContext.AllContexts() )
                .SelectMany( contexts => contexts.AllContexts().Where( context => context.Name == name ) ).First();

            theContext.Name.should_be( name );

            return theContext;
        }

        protected Example TheExample( string name )
        {
            var theExample = contextCollection
                .SelectMany( rootContext => rootContext.AllContexts() )
                .SelectMany( contexts => contexts.AllExamples().Where( example => example.Spec == name ) ).First();

            theExample.Spec.should_be( name );

            return theExample;
        }

        protected ContextCollection contextCollection;
        protected ClassContext classContext;
        protected Context methodContext;
    }
}