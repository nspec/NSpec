using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class ActChain : HookChainBase
    {
        public override void BuildMethodLevel(Conventions conventions, List<Type> classHierarchy)
        {
            var methods = GetMethodsFromHierarchy(
                classHierarchy, conventions.GetMethodLevelAct);

            if (methods.Count > 0)
            {
                ClassHook = instance => methods.Do(m => m.Invoke(instance, null));
            }

            var asyncMethods = GetMethodsFromHierarchy(
                classHierarchy, conventions.GetAsyncMethodLevelAct);

            if (asyncMethods.Count > 0)
            {
                AsyncClassHook = instance => asyncMethods.Do(m => new AsyncMethodLevelAct(m).Run(instance));
            }
        }

        protected override void RunHooks(nspec instance)
        {
            // parent chain

            RecurseAncestors(c => c.ActChain.RunHooks(instance));

            // class (method-level)

            if (ClassHook != null && AsyncClassHook != null)
            {
                throw new AsyncMismatchException(
                    "A spec class with all its ancestors cannot set both sync and async class-level 'act_each' hooks, they should either be all sync or all async");
            }

            ClassHook.SafeInvoke(instance);

            AsyncClassHook.SafeInvoke(instance);

            // context-level

            if (Hook != null && AsyncHook != null)
            {
                throw new AsyncMismatchException(
                    "A single context cannot set both an 'act' and an 'actAsync', please pick one of the two");
            }

            if (Hook != null && Hook.IsAsync())
            {
                throw new AsyncMismatchException(
                    "'act' cannot be set to an async delegate, please use 'actAsync' instead");
            }

            Hook.SafeInvoke();

            AsyncHook.SafeInvoke();
        }

        protected override bool CanRun(nspec instance)
        {
            return context.BeforeAllChain.AnyBeforeAllsThrew()
                ? false
                : (context.BeforeChain.Exception == null);
        }

        public ActChain(Context context) : base(context)
        { }
    }
}
