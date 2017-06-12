using System;

namespace NSpec.Domain
{
    public abstract class TraversingHookChain : HookChainBase
    {
        protected override void InvokeHooks(nspec instance)
        {
            if (!reversed)
            {
                // parent chain first, then current chain

                RecurseAncestors(c => chainSelector(c).InvokeHooks(instance));

                base.InvokeHooks(instance);
            }
            else
            {
                // current chain first, then parent chain

                base.InvokeHooks(instance);

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
