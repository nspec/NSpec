using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class AfterChain : HookChainBase
    {
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

            // parent chain
            RecurseAncestors(c => c.AfterChain.RunHooks(instance));
        }

        protected override bool CanRun(nspec instance)
        {
            return !context.BeforeAllChain.AnyBeforeAllsThrew();
        }

        public AfterChain(Context context, Conventions conventions)
            : base(context, "after", "afterAsync", "after_each")
        {
            methodSelector = conventions.GetMethodLevelAfter;
            asyncMethodSelector = conventions.GetAsyncMethodLevelAfter;
        }
    }
}
