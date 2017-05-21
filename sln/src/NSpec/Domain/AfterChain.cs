using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class AfterChain : HookChainBase
    {
        protected override bool CanRun(nspec instance)
        {
            return !context.BeforeAllChain.AnyBeforeAllsThrew();
        }

        public AfterChain(Context context, Conventions conventions)
            : base(context, true, true, "after", "afterAsync", "after_each")
        {
            methodSelector = conventions.GetMethodLevelAfter;
            asyncMethodSelector = conventions.GetAsyncMethodLevelAfter;
            chainSelector = c => c.AfterChain;
        }
    }
}
