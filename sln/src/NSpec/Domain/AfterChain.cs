using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class AfterChain : TraversingHookChain
    {
        protected override bool CanRun(nspec instance)
        {
            return !context.BeforeAllChain.AnyBeforeAllsThrew();
        }

        public AfterChain(Context context, Conventions conventions)
            : base(context, "after", "afterAsync", "after_each", reversed: true)
        {
            methodSelector = conventions.GetMethodLevelAfter;
            asyncMethodSelector = conventions.GetAsyncMethodLevelAfter;
            chainSelector = c => c.AfterChain;
        }
    }
}
