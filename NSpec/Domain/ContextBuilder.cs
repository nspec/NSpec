using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class ContextBuilder : IContextBuilder
    {
        public ContextBuilder(ISpecFinder finder, string spec = "")
        {
            this.finder = finder;

            contexts = new List<Context>();
        }

        public IList<Context> Contexts()
        {
            contexts.Clear();
            finder.SpecClasses().Do(Build);
            return contexts;
        }

        private void Build(Type specClass)
        {
            var root = specClass.RootContext();

            var parent = contexts.FirstOrDefault(c=>c.Name==root.Name);

            var classContext = root.SelfAndDescendants().First(c => c.Type == specClass);

            if(parent == null) 
                contexts.Add(root);
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

        private IList<Context> contexts;
    }

    public interface IContextBuilder
    {
        IList<Context> Contexts();
    }
}