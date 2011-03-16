using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class ContextBuilder2 : List<Context>
    {
        public ContextBuilder2(ISpecFinder finder, string spec = "")
        {
            this.finder = finder;

            finder.SpecClasses(spec).Do(Build);
        }

        private void Build(Type specClass)
        {
            var root = specClass.RootContext();

            var parent = this.FirstOrDefault(c=>c.Name==root.Name);

            var classContext = root.SelfAndDescendants().First(c => c.Type == specClass);

            if(parent == null) 
                this.Add(root);
            else
                parent.AddContext(classContext);

            BuildMethodContexts(classContext, specClass);
        }

        public void BuildMethodContexts(Context classContext, Type specClass)
        {
            specClass.Methods(finder.Except).Do(contextMethod =>
                                                    {
                                                        var methodContext = new Context(contextMethod);

                                                        classContext.AddContext(methodContext);
                                                    });
        }

        private readonly ISpecFinder finder;
        private readonly string spec;
    }
}