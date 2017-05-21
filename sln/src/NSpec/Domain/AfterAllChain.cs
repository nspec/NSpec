using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class AfterAllChain : HookChainBase
    {
        protected override bool CanRun(nspec instance)
        {
            return context.BeforeAllChain.AncestorBeforeAllsThrew()
                ? false
                : context.AnyUnfilteredExampleInSubTree(instance);
        }

        public AfterAllChain(Context context, Conventions conventions)
            : base(context, "afterAll", "afterAllAsync", "after_all", reversed: true)
        {
            methodSelector = conventions.GetMethodLevelAfterAll;
            asyncMethodSelector = conventions.GetAsyncMethodLevelAfterAll;
        }
    }
}
