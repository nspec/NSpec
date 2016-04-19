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

        ClassContext CreateClassContext(Type type)
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

        // Note: this is public only because of unit tests
        public void BuildMethodContexts(Context classContext, Type specClass)
        {
            specClass
                .Methods()
                .Where(s => conventions.IsMethodLevelContext(s.Name))
                .Do(contextMethod =>
                {
                    var methodContext = new MethodContext(contextMethod, TagStringFor(contextMethod));

                    classContext.AddContext(methodContext);
                });
        }

        void BuildMethodLevelExamples(Context classContext, Type specClass)
        {
            Func<MethodInfo, MethodExampleBase> buildMethodLevel = method =>
                new MethodExample(method, TagStringFor(method));

            Func<MethodInfo, MethodExampleBase> buildAsyncMethodLevel = method =>
                new AsyncMethodExample(method, TagStringFor(method));

            specClass
                .Methods()
                .Union(specClass
                    .AsyncMethods())
                .Where(method => conventions.IsMethodLevelExample(method.Name))
                .Select(method =>
                {
                    return method.IsAsync()
                        ? buildAsyncMethodLevel(method)
                        : buildMethodLevel(method);
                })
                .Do(methodExample =>
                {
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

        public ContextBuilder(ISpecFinder finder, Tags tagsFilter)
            : this(finder, tagsFilter, new DefaultConventions()) { }

        public ContextBuilder(ISpecFinder finder, Conventions conventions)
            : this(finder, new Tags(), conventions) { }

        public ContextBuilder(ISpecFinder finder, Tags tagsFilter, Conventions conventions)
        {
            contexts = new ContextCollection();

            this.finder = finder;

            this.conventions = conventions;

            this.tagsFilter = tagsFilter;
        }

        Tags tagsFilter;

        Conventions conventions;

        ISpecFinder finder;

        ContextCollection contexts;
    }
}