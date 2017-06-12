using System;

namespace NSpec.Domain
{
    public class BeforeAllChain : HookChainBase
    {
        protected override bool CanRun(nspec instance)
        {
            return AncestorsThrew()
                ? false
                : context.AnyUnfilteredExampleInSubTree(instance);
        }

        public bool AnyThrew()
        {
            return (Exception != null || AncestorsThrew());
        }

        public bool AncestorsThrew()
        {
            return (context.Parent?.BeforeAllChain.AnyThrew() ?? false);
        }

        public override Exception AnyException()
        {
            // when hook chain is NOT traversed, build up exceptions along ancestor chain

            // give precedence to Exception farther up in the chain
            return context.Parent?.BeforeAllChain.AnyException() ?? Exception;
        }

        public BeforeAllChain(Context context, Conventions conventions)
            : base(context, "beforeAll", "beforeAllAsync", "before_all")
        {
            methodSelector = conventions.GetMethodLevelBeforeAll;
            asyncMethodSelector = conventions.GetAsyncMethodLevelBeforeAll;
        }
    }
}
