using System;

namespace NSpec.Domain
{
    public class AfterAllChain : HookChainBase
    {
        protected override bool CanRun(nspec instance)
        {
            return context.BeforeAllChain.AncestorsThrew()
                ? false
                : context.AnyUnfilteredExampleInSubTree(instance);
        }

        public bool AnyThrew()
        {
            return (AnyException() != null);
        }

        public override Exception AnyException()
        {
            // when hook chain is NOT traversed, build up exceptions along ancestor chain

            // give precedence to Exception closer in the chain
            return Exception ?? context.Parent?.AfterAllChain.AnyException();
        }

        public AfterAllChain(Context context, Conventions conventions)
            : base(context, "afterAll", "afterAllAsync", "after_all", reversed: true)
        {
            methodSelector = conventions.GetMethodLevelAfterAll;
            asyncMethodSelector = conventions.GetAsyncMethodLevelAfterAll;
        }
    }
}
