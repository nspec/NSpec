using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ActChain : HookChainBase
    {
        protected override Func<Type, MethodInfo> GetMethodSelector(Conventions conventions)
        {
            return conventions.GetMethodLevelAct;
        }

        protected override Func<Type, MethodInfo> GetAsyncMethodSelector(Conventions conventions)
        {
            return conventions.GetAsyncMethodLevelAct;
        }

        protected override void RunHooks(nspec instance)
        {
            // parent chain
            RecurseAncestors(c => c.ActChain.RunHooks(instance));

            // class (method-level)
            RunClassHooks(instance);

            // context-level
            RunContextHooks();
        }

        protected override bool CanRun(nspec instance)
        {
            return context.BeforeAllChain.AnyBeforeAllsThrew()
                ? false
                : (context.BeforeChain.Exception == null);
        }

        public ActChain(Context context) : base(context,
            "act", "actAsync", "act_each")
        { }
    }
}
