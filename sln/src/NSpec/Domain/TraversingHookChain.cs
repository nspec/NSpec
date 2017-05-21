using System;

namespace NSpec.Domain
{
    public abstract class TraversingHookChain : HookChainBase
    {
        protected override void InvokeHooks(nspec instance)
        {
            // parent chain
            if (!reversed)
            {
                RecurseAncestors(c => chainSelector(c).InvokeHooks(instance));
            }

            base.InvokeHooks(instance);

            // parent chain, reverse order
            if (reversed)
            {
                RecurseAncestors(c => chainSelector(c).InvokeHooks(instance));
            }
        }

        protected void RecurseAncestors(Action<Context> ancestorAction)
        {
            if (context.Parent != null) ancestorAction(context.Parent);
        }

        public override Exception AnyException()
        {
            // when hook chain is traversed, this chain exception holds any ancestor exception

            return Exception;
        }

        public bool AnyThrew()
        {
            // when hook chain is traversed, this chain exception holds any ancestor exception

            return (Exception != null);
        }

        public TraversingHookChain(Context context,
            string hookName, string asyncHookName, string classHookName, bool reversed = false)
            : base(context, hookName, asyncHookName, classHookName, reversed)
        { }

        protected Func<Context, TraversingHookChain> chainSelector;
    }
}
