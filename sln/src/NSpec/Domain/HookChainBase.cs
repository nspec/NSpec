using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public abstract class HookChainBase
    {
        public void BuildMethodLevel(List<Type> classHierarchy)
        {
            var methods = ContextUtils.GetMethodsFromHierarchy(classHierarchy, methodSelector);

            if (reversed)
            {
                methods.Reverse();
            }

            if (methods.Count > 0)
            {
                ClassHook = instance => methods.Do(m => m.Invoke(instance, null));
            }

            var asyncMethods = ContextUtils.GetMethodsFromHierarchy(classHierarchy, asyncMethodSelector);

            if (reversed)
            {
                asyncMethods.Reverse();
            }

            if (asyncMethods.Count > 0)
            {
                AsyncClassHook = instance => asyncMethods.Do(m => new AsyncMethodLevelBefore(m).Run(instance));
            }
        }

        protected abstract bool CanRun(nspec instance);

        public void Run(nspec instance)
        {
            if (CanRun(instance))
            {
                ContextUtils.RunAndHandleException(InvokeHooks, instance, ref exception);
            }
        }

        protected virtual void InvokeHooks(nspec instance)
        {
            // class (method-level)
            InvokeClassHooks(instance);

            // context-level
            InvokeContextHooks();
        }

        protected void InvokeClassHooks(nspec instance)
        {
            // class (method-level)

            if (ClassHook != null && AsyncClassHook != null)
            {
                throw new AsyncMismatchException(
                    $"A spec class with all its ancestors cannot set both sync and async " +
                    $"class-level '{classHookName}' hooks, they should either be all sync or all async");
            }

            ClassHook.SafeInvoke(instance);

            AsyncClassHook.SafeInvoke(instance);
        }

        protected void InvokeContextHooks()
        {
            // context-level

            if (Hook != null && AsyncHook != null)
            {
                throw new AsyncMismatchException(
                    $"A single context cannot set both a '{hookName}' and an '{asyncHookName}', please pick one of the two");
            }

            if (Hook != null && Hook.IsAsync())
            {
                throw new AsyncMismatchException(
                    $"'{hookName}' cannot be set to an async delegate, please use '{asyncHookName}' instead");
            }

            Hook.SafeInvoke();

            AsyncHook.SafeInvoke();
        }

        public Exception Exception
        {
            get { return exception; }
            protected set { exception = value; }
        }

        public abstract Exception AnyException();

        public HookChainBase(Context context,
            string hookName, string asyncHookName, string classHookName, bool reversed = false)
        {
            this.context = context;
            this.hookName = hookName;
            this.asyncHookName = asyncHookName;
            this.classHookName = classHookName;
            this.reversed = reversed;
        }

        public Action Hook;
        public Func<Task> AsyncHook;

        public Action<nspec> ClassHook { get; protected set; }
        public Action<nspec> AsyncClassHook { get; protected set; }

        protected Exception exception;
        protected Func<Type, MethodInfo> methodSelector;
        protected Func<Type, MethodInfo> asyncMethodSelector;

        protected readonly Context context;
        protected readonly bool reversed;
        protected readonly string hookName;
        protected readonly string asyncHookName;
        protected readonly string classHookName;
    }
}
