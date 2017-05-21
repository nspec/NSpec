using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class BeforeAllChain : HookChainBase
    {
        protected override void RunHooks(nspec instance)
        {
            // do NOT traverse parent chain

            // class (method-level)
            RunClassHooks(instance);

            // context-level
            RunContextHooks();
        }

        protected override bool CanRun(nspec instance)
        {
            return AncestorBeforeAllsThrew()
                ? false
                : context.AnyUnfilteredExampleInSubTree(instance);
        }

        public bool AnyBeforeAllsThrew()
        {
            return (Exception != null || AncestorBeforeAllsThrew());
        }

        public bool AncestorBeforeAllsThrew()
        {
            return (context.Parent?.BeforeAllChain.AnyBeforeAllsThrew() ?? false);
        }

        public BeforeAllChain(Context context, Conventions conventions)
            : base(context, "beforeAll", "beforeAllAsync", "before_all")
        {
            methodSelector = conventions.GetMethodLevelBeforeAll;
            asyncMethodSelector = conventions.GetAsyncMethodLevelBeforeAll;
        }
    }
}
