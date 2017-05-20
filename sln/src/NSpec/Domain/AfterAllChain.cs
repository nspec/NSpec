using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class AfterAllChain : HookChainBase
    {
        protected override Func<Type, MethodInfo> GetMethodSelector(Conventions conventions)
        {
            return conventions.GetMethodLevelAfterAll;
        }

        protected override Func<Type, MethodInfo> GetAsyncMethodSelector(Conventions conventions)
        {
            return conventions.GetAsyncMethodLevelAfterAll;
        }

        protected override bool ReverseClassMethods()
        {
            return true;
        }

        protected override void RunHooks(nspec instance)
        {
            // context-level
            RunContextHooks();

            // class (method-level)
            RunClassHooks(instance);

            // do NOT traverse parent chain
        }

        protected override bool CanRun(nspec instance)
        {
            return context.BeforeAllChain.AncestorBeforeAllsThrew()
                ? false
                : context.AnyUnfilteredExampleInSubTree(instance);
        }

        public AfterAllChain(Context context) : base(context,
            "afterAll", "afterAllAsync", "after_all")
        { }
    }
}
