using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ActChain : HookChainBase
    {
        protected override bool CanRun(nspec instance)
        {
            return context.BeforeAllChain.AnyBeforeAllsThrew()
                ? false
                : (context.BeforeChain.Exception == null);
        }

        public ActChain(Context context, Conventions conventions)
            : base(context, true, false, "act", "actAsync", "act_each")
        {
            methodSelector = conventions.GetMethodLevelAct;
            asyncMethodSelector = conventions.GetAsyncMethodLevelAct;
            chainSelector = c => c.ActChain;
        }
    }
}
