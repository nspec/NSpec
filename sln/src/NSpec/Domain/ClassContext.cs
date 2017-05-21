using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ClassContext : Context
    {
        public override void Build(nspec unused = null)
        {
            BeforeAllChain.BuildMethodLevel(classHierarchy);

            BeforeChain.BuildMethodLevel(classHierarchy);

            ActChain.BuildMethodLevel(classHierarchy);

            AfterChain.BuildMethodLevel(classHierarchy);

            AfterAllChain.BuildMethodLevel(classHierarchy);

            try
            {
                var nspec = SpecType.CreateInstanceAs<nspec>();

                nspec.tagsFilter = tagsFilter ?? new Tags();

                base.Build(nspec);
            }
            catch (Exception ex)
            {
                cantCreateInstance = true;

                AddFailingExample(ex);
            }
        }

        public override bool IsSub(Type baseType)
        {
            while (baseType != null && baseType.GetTypeInfo().IsAbstract)
            {
                baseType = baseType.GetTypeInfo().BaseType;
            }

            return baseType == SpecType;
        }

        IEnumerable<MethodInfo> GetMethodsFromHierarchy(Func<Type, MethodInfo> methodAccessor)
        {
            return classHierarchy.Select(methodAccessor).Where(mi => mi != null);
        }

        void AddFailingExample(Exception targetEx)
        {
            var reportedEx = (targetEx.InnerException != null)
                ? targetEx.InnerException
                : targetEx;

            string exampleName = "Constructor in spec class '{0}' throws an exception of type '{1}'"
                .With(SpecType.FullName, reportedEx.GetType().Name);

            Action emptyAction = () => { };

            var failingExample = new Example(exampleName, action: emptyAction)
            {
                Exception = new ContextBareCodeException(reportedEx),
            };

            this.AddExample(failingExample);
        }

        public override void Run(bool failFast, nspec instance = null, bool recurse = true)
        {
            if (cantCreateInstance)
            {
                // flag the one and only failing example as being run;
                // nothing else is needed: no parents, no childs, no before/after hooks
                Examples.Single().HasRun = true;
            }
            else
            {
                base.Run(failFast, instance, recurse);
            }
        }

        public ClassContext(Type type, Conventions conventions = null, Tags tagsFilter = null, string tags = null)
            : base(type.CleanName(), tags, false, conventions)
        {
            this.SpecType = type;

            this.tagsFilter = tagsFilter;

            this.classHierarchy = (type == typeof(nspec))
                ? new List<Type>()
                : new List<Type>(type.GetAbstractBaseClassChainWithClass());
        }

        public Type SpecType;

        Tags tagsFilter;
        List<Type> classHierarchy;
        bool cantCreateInstance = false;
    }
}
