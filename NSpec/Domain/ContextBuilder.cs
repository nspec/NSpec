using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    [Serializable]
    public class ContextBuilder
    {
        public ContextCollection Contexts()
        {
            contexts.Clear();

            conventions.Initialize();

            var specClasses = finder.SpecClasses();

            var container = new ClassContext(typeof(nspec), conventions, tagsFilter);

            Build(container, specClasses);

            contexts.AddRange(container.Contexts);

            return contexts;
        }

        /// <summary>
        /// Builds contexts for a single function instead of an entire class
        /// </summary>
        /// <param name="method">The MethodInfo of the function to build contexts for</param>
        /// <returns>A context collection for the function</returns>
        public ContextCollection MethodContext(MethodInfo method)
        {
            // set the method to restrict to
            methodToRestrictTo = method;

            // build the contexts normal, but we know it will be a collection of one
            var context = Contexts();

            // reset the method to restrict to
            methodToRestrictTo = null;

            return context;

            if (context[0] != null && context[0].Contexts[0] != null)
            {
                return context[0].Contexts;
            }
            else
            {
                return new ContextCollection();
            }
        }

        public ClassContext CreateClassContext(Type type)
        {
            var tagAttributes = ((TagAttribute[])type.GetCustomAttributes(typeof(TagAttribute), false)).ToList();

            tagAttributes.Add(new TagAttribute(type.Name));

            type.GetAbstractBaseClassChainWithClass()
                .Where(s => s != type)
                .Each(s => tagAttributes.Add(new TagAttribute(s.Name)));

            var tags = TagStringFor(tagAttributes);

            var context = new ClassContext(type, conventions, tagsFilter, tags);

            return context;
        }

        public void BuildMethodContexts(Context classContext, Type specClass)
        {
            specClass
                .Methods()
                .Where(s => conventions.IsMethodLevelContext(s.Name) && (methodToRestrictTo == null || (methodToRestrictTo != null && s == methodToRestrictTo)))
                .Do(contextMethod =>
                {
                    var methodContext = new MethodContext(contextMethod, TagStringFor(contextMethod));

                    classContext.AddContext(methodContext);
                });
        }

        public void BuildMethodLevelExamples(Context classContext, Type specClass)
        {
            specClass
                .Methods()
                .Where(s => conventions.IsMethodLevelExample(s.Name) && (methodToRestrictTo == null || (methodToRestrictTo != null && s == methodToRestrictTo)))
                .Do(methodInfo =>
                {
                    var methodExample = new MethodExample(methodInfo, TagStringFor(methodInfo));

                    classContext.AddExample(methodExample);
                });
        }

        void Build(Context parent, IEnumerable<Type> allSpecClasses)
        {
            var derivedTypes = allSpecClasses.Where(s => parent.IsSub(s.BaseType));

            foreach (var derived in derivedTypes)
            {
                var classContext = CreateClassContext(derived);

                parent.AddContext(classContext);

                BuildMethodContexts(classContext, derived);

                BuildMethodLevelExamples(classContext, derived);

                Build(classContext, allSpecClasses);
            }
        }

        string TagStringFor(MethodInfo method)
        {
            return TagStringFor(TagAttributesFor(method));
        }

        string TagStringFor(IEnumerable<TagAttribute> tagAttributes)
        {
            return tagAttributes.Aggregate("", (current, tagAttribute) => current + (", " + tagAttribute.Tags));
        }

        IEnumerable<TagAttribute> TagAttributesFor(MethodInfo method)
        {
            return (TagAttribute[])method.GetCustomAttributes(typeof(TagAttribute), false);
        }

        public ContextBuilder()
        {
            contexts = new ContextCollection();
            conventions = new DefaultConventions();
            tagsFilter = new Tags();
        }

        public ContextBuilder(ISpecFinder finder, Tags tagsFilter)
            : this(finder, new DefaultConventions()) { }

        public ContextBuilder(ISpecFinder finder, Conventions conventions)
            : this(finder, new Tags(), conventions) { }

        public ContextBuilder(ISpecFinder finder, Tags tagsFilter, Conventions conventions)
        {
            this.contexts = new ContextCollection();

            this.finder = finder;

            this.conventions = conventions;

            this.tagsFilter = tagsFilter;
        }

        public Tags tagsFilter;

        private Conventions conventions;

        private ISpecFinder finder;

        private ContextCollection contexts;

        private MethodInfo methodToRestrictTo;

        private List<nspec> classesThatHaveBeenRun = new List<nspec>(); 
    }
}