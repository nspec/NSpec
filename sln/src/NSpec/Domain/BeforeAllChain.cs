using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class BeforeAllChain : HookChainBase
    {
        protected override bool CanRun(nspec instance)
        {
            return AncestorBeforeAllsThrew()
                ? false
                : context.AnyUnfilteredExampleInSubTree(instance);
        }

        public bool AnyBeforeAllThrew()
        {
            return (Exception != null || AncestorBeforeAllsThrew());
        }

        public bool AncestorBeforeAllsThrew()
        {
            return (context.Parent?.BeforeAllChain.AnyBeforeAllThrew() ?? false);
        }

        public Exception AnyBeforeAllException()
        {
            // give precedence to Exception farther up in the chain

            return context.Parent?.BeforeAllChain.AnyBeforeAllException() ?? Exception;
        }

        public BeforeAllChain(Context context, Conventions conventions)
            : base(context, "beforeAll", "beforeAllAsync", "before_all")
        {
            methodSelector = conventions.GetMethodLevelBeforeAll;
            asyncMethodSelector = conventions.GetAsyncMethodLevelBeforeAll;
        }
    }
}
