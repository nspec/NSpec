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
            : base(context, false, true, "afterAll", "afterAllAsync", "after_all")
        {
            methodSelector = conventions.GetMethodLevelAfterAll;
            asyncMethodSelector = conventions.GetAsyncMethodLevelAfterAll;
            //chainSelector = c => c.AfterAllChain;
        }
    }
}
