using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Domain.Extensions;
using System.Text.RegularExpressions;

namespace NSpec.Domain
{
    public class ContextBuilder : IContextBuilder
    {
        public ContextBuilder(ISpecFinder finder)
        {
            this.finder = finder;

            contexts = new List<Context>();
        }

        public IList<Context> Contexts()
        {
            contexts.Clear();

            var specClasses = finder.SpecClasses();

            var container = new ClassContext(typeof(nspec));

            Build(container, specClasses);

            contexts.AddRange(container.Contexts);

            return contexts;
        }

        private void Build(Context parent, IEnumerable<Type> allSpecClasses)
        {
            var derivedTypes = allSpecClasses.Where(s => s.BaseType == parent.Type);

            foreach (var derived in derivedTypes)
            {
                var classContext = CreateClassContext(derived);

                parent.AddContext(classContext);

                Build(classContext, allSpecClasses);
            }
        }

        private ClassContext CreateClassContext(Type type)
        {
            var context = new ClassContext(type);

            BuildMethodContexts(context, type);

            BuildMethodLevelExamples(context, type);

            return context;
        }

        public void BuildMethodContexts(Context classContext, Type specClass)
        {
            specClass
                .Methods()
                .Where(s => IsMethodLevelContext(s.Name)).Do(
                contextMethod =>
                {
                    var methodContext = new MethodContext(contextMethod);

                    classContext.AddContext(methodContext);
                });
        }

        public void BuildMethodLevelExamples(Context classContext, Type specClass)
        {
            specClass
                .Methods()
                .Where(s => IsMethodLevelExample(s.Name)).Do(
                methodInfo =>
                {
                    var example = new Example(methodInfo);

                    classContext.AddExample(example);
                });
        }

        private bool IsMethodLevelContext(string name)
        {
            return !reservedMethods.Contains(name) && !IsMethodLevelExample(name);
        }

        private bool IsMethodLevelExample(string name)
        {
            return Regex.IsMatch(name.ToLower(), "(^it_)|(^specify_)");
        }

        private readonly ISpecFinder finder;

        private readonly string[] reservedMethods = new[] { "before_each", "act_each" };

        private List<Context> contexts;
    }

    public interface IContextBuilder
    {
        IList<Context> Contexts();
    }
}