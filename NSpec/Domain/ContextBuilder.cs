using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ContextBuilder
    {
        public ContextBuilder(ISpecFinder finder, Conventions conventions)
            : this(finder, null, conventions)
        {
        }

        public ContextBuilder( ISpecFinder finder, Tags tagsFilter, Conventions conventions )
        {
            contexts = new ContextCollection();

            this.finder = finder;

            this.conventions = conventions;

            this.tagsFilter = tagsFilter;
        }

        public ContextCollection Contexts()
        {
            contexts.Clear();

            conventions.Initialize();

            var specClasses = finder.SpecClasses();

            var container = new ClassContext( typeof( nspec ), conventions, tagsFilter );

            Build( container, specClasses );

            contexts.AddRange( container.Contexts );

            return contexts;
        }

        private void Build(Context parent, IEnumerable<Type> allSpecClasses)
        {
            var derivedTypes = allSpecClasses.Where(s => parent.IsSub( s.BaseType) );

            foreach( var derived in derivedTypes )
            {
                var classContext = CreateClassContext(derived);

                parent.AddContext(classContext);

                Build(classContext, allSpecClasses);
            }
        }

        public ClassContext CreateClassContext(Type type)
        {
            // extract tags as string from class-level attribute(s)
            var tagAttributes = (TagAttribute[]) type.GetCustomAttributes( typeof( TagAttribute ), false );
            var tags = tagAttributes.Aggregate( "", ( current, tagAttribute ) => current + ( ", " + tagAttribute.Tags ) );

            var context = new ClassContext( type, conventions, tagsFilter, tags );

            BuildMethodContexts( context, type );

            BuildMethodLevelExamples(context, type);

            return context;
        }

        public void BuildMethodContexts(Context classContext, Type specClass)
        {
            specClass
                .Methods()
                .Where(s => conventions.IsMethodLevelContext(s.Name)).Do(
                contextMethod =>
                {
                    // extract tags as string from method-level attribute(s)
                    var tagAttributes = (TagAttribute[]) contextMethod.GetCustomAttributes( typeof( TagAttribute ), false );
                    var tags = tagAttributes.Aggregate( "", ( current, tagAttribute ) => current + ( ", " + tagAttribute.Tags ) );

                    var methodContext = new MethodContext( contextMethod, tags );

                    // inherit tags from parent context
                    methodContext.Tags.AddRange( classContext.Tags );

                    classContext.AddContext( methodContext );
                });
        }

        public void BuildMethodLevelExamples(Context classContext, Type specClass)
        {
            specClass
                .Methods()
                .Where(s => conventions.IsMethodLevelExample(s.Name)).Do(
                methodInfo =>
                {
                    // extract tags as string from method-level attribute(s)
                    var tagAttributes = (TagAttribute[]) methodInfo.GetCustomAttributes( typeof( TagAttribute ), false );
                    var tags = tagAttributes.Aggregate( "", ( current, tagAttribute ) => current + ( ", " + tagAttribute.Tags ) );

                    var methodExample = new Example( methodInfo, tags );

                    // inherit tags from parent context
                    methodExample.Tags.AddRange( classContext.Tags );

                    // skip examples if no "include tags" are present in example
                    if( tagsFilter != null && !tagsFilter.IncludesAny( methodExample.Tags ) )
                        return;

                    // skip examples if any "skip tags" are present in example
                    if( tagsFilter != null && tagsFilter.ExcludesAny( methodExample.Tags ) )
                        return;

                    classContext.AddExample( methodExample );
                });
        }

        private Conventions conventions;
        
        private ISpecFinder finder;

        private ContextCollection contexts;

        public Tags tagsFilter;
    }
}