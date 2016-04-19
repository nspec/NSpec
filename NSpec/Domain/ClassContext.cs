using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ClassContext : Context
    {
        public override void Build(nspec instance = null)
        {
            BuildMethodLevelBefore();

            BuildMethodLevelBeforeAll();

            BuildMethodLevelAct();

            BuildMethodLevelAfter();

            BuildMethodLevelAfterAll();

            var nspec = SpecType.CreateInstanceAs<nspec>();

            nspec.tagsFilter = tagsFilter ?? new Tags();

            base.Build(nspec);
        }

        public override bool IsSub(Type baseType)
        {
            while (baseType != null && baseType.IsAbstract)
            {
                baseType = baseType.BaseType;
            }

            return baseType == SpecType;
        }

        IEnumerable<MethodInfo> GetMethodsFromHierarchy(Func<Type, MethodInfo> methodAccessor)
        {
            return classHierarchyToClass.Select(methodAccessor).Where(mi => mi != null);
        }

        void BuildMethodLevelBefore()
        {
            var befores = GetMethodsFromHierarchy(conventions.GetMethodLevelBefore).ToList();

            if (befores.Count > 0)
            {
                BeforeInstance = instance => befores.Do(b => b.Invoke(instance, null));
            }

            var asyncBefores = GetMethodsFromHierarchy(conventions.GetAsyncMethodLevelBefore).ToList();

            if (asyncBefores.Count > 0)
            {
                BeforeInstanceAsync = instance => asyncBefores.Do(b => new AsyncMethodLevelBefore(b).Run(instance));
            }
        }

        void BuildMethodLevelBeforeAll()
        {
            var beforeAlls = GetMethodsFromHierarchy(conventions.GetMethodLevelBeforeAll).ToList();

            if (beforeAlls.Count > 0)
            {
                BeforeAllInstance = instance => beforeAlls.Do(a => a.Invoke(instance, null));
            }

            var asyncBeforeAlls = GetMethodsFromHierarchy(conventions.GetAsyncMethodLevelBeforeAll).ToList();

            if (asyncBeforeAlls.Count > 0)
            {
                BeforeAllInstanceAsync = instance => asyncBeforeAlls.Do(b => new AsyncMethodLevelBeforeAll(b).Run(instance));
            }
        }

        void BuildMethodLevelAct()
        {
            var acts = GetMethodsFromHierarchy(conventions.GetMethodLevelAct).ToList();

            if (acts.Count > 0)
            {
                ActInstance = instance => acts.Do(a => a.Invoke(instance, null));
            }

            var asyncActs = GetMethodsFromHierarchy(conventions.GetAsyncMethodLevelAct).ToList();

            if (asyncActs.Count > 0)
            {
                ActInstanceAsync = instance => asyncActs.Do(a => new AsyncMethodLevelAct(a).Run(instance));
            }
        }

        void BuildMethodLevelAfter()
        {
            var afters = GetMethodsFromHierarchy(conventions.GetMethodLevelAfter).Reverse().ToList();

            if (afters.Count > 0)
            {
                AfterInstance = instance => afters.Do(a => a.Invoke(instance, null));
            }

            var asyncAfters = GetMethodsFromHierarchy(conventions.GetAsyncMethodLevelAfter).Reverse().ToList();

            if (asyncAfters.Count > 0)
            {
                AfterInstanceAsync = instance => asyncAfters.Do(a => new AsyncMethodLevelAfter(a).Run(instance));
            }
        }

        void BuildMethodLevelAfterAll()
        {
            var afterAlls = GetMethodsFromHierarchy(conventions.GetMethodLevelAfterAll).Reverse().ToList();

            if (afterAlls.Count > 0)
            {
                AfterAllInstance = instance => afterAlls.Do(a => a.Invoke(instance, null));
            }

            var asyncAfterAlls = GetMethodsFromHierarchy(conventions.GetAsyncMethodLevelAfterAll).Reverse().ToList();

            if (asyncAfterAlls.Count > 0)
            {
                AfterAllInstanceAsync = instance => asyncAfterAlls.Do(a => new AsyncMethodLevelAfterAll(a).Run(instance));
            }
        }

        public ClassContext(Type type, Conventions conventions = null, Tags tagsFilter = null, string tags = null)
            : base(type.CleanName(), tags)
        {
            this.SpecType = type;

            this.conventions = conventions ?? new DefaultConventions().Initialize();

            this.tagsFilter = tagsFilter;

            if (type != typeof(nspec))
            {
                classHierarchyToClass.AddRange(type.GetAbstractBaseClassChainWithClass());
            }
        }

        public Type SpecType;

        Tags tagsFilter;
        List<Type> classHierarchyToClass = new List<Type>();
        Conventions conventions;
    }
}