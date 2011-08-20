using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ContextBuilder
    {
        public ContextBuilder(ISpecFinder finder, Conventions conventions)
        {
            this.finder = finder;

            contexts = new ContextCollection();

            this.conventions = conventions;
        }

        public ContextCollection Contexts()
        {
            contexts.Clear();

            conventions.Initialize();

            var specClasses = finder.SpecClasses();

            var container = new ClassContext(typeof(nspec), conventions);

            Build(container, specClasses);

            contexts.AddRange(container.Contexts);

            return contexts;
        }

        private void Build(Context parent, IEnumerable<Type> allSpecClasses)
        {
            var derivedTypes = allSpecClasses.Where(s => parent.IsSub( s.BaseType) );

            foreach (var derived in derivedTypes)
            {
                var classContext = CreateClassContext(derived);

                parent.AddContext(classContext);

                Build(classContext, allSpecClasses);
            }
        }

        public ClassContext CreateClassContext(Type type)
        {
            var context = new ClassContext(type, conventions);

            BuildMethodContexts(context, type);

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
                    classContext.AddContext(new MethodContext(contextMethod));
                });
        }

        public void BuildMethodLevelExamples(Context classContext, Type specClass)
        {
            specClass
                .Methods()
                .Where(s => conventions.IsMethodLevelExample(s.Name)).Do(
                methodInfo =>
                {
                    classContext.AddExample(new Example(methodInfo));
                });
        }

        private Conventions conventions;
        
        private ISpecFinder finder;

        private ContextCollection contexts;
    }
}