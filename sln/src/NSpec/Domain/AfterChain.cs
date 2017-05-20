using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class AfterChain : HookChainBase
    {
        protected override Func<Type, MethodInfo> GetMethodSelector(Conventions conventions)
        {
            return conventions.GetMethodLevelAfter;
        }

        protected override Func<Type, MethodInfo> GetAsyncMethodSelector(Conventions conventions)
        {
            return conventions.GetAsyncMethodLevelAfter;
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

            // parent chain
            RecurseAncestors(c => c.AfterChain.RunHooks(instance));
        }

        protected override bool CanRun(nspec instance)
        {
            return !context.BeforeAllChain.AnyBeforeAllsThrew();
        }

        public AfterChain(Context context) : base(context,
            "after", "afterAsync", "after_each")
        { }
    }
}
