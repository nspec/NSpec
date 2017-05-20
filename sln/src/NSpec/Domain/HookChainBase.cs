using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public abstract class HookChainBase
    {
        public void BuildMethodLevel(Conventions conventions, List<Type> classHierarchy)
        {
            var selector = GetMethodSelector(conventions);
            var methods = GetMethodsFromHierarchy(classHierarchy, selector);

            if (ReverseClassMethods())
            {
                methods.Reverse();
            }

            if (methods.Count > 0)
            {
                ClassHook = instance => methods.Do(m => m.Invoke(instance, null));
            }

            var asyncSelector = GetAsyncMethodSelector(conventions);
            var asyncMethods = GetMethodsFromHierarchy(classHierarchy, asyncSelector);

            if (ReverseClassMethods())
            {
                asyncMethods.Reverse();
            }

            if (asyncMethods.Count > 0)
            {
                AsyncClassHook = instance => asyncMethods.Do(m => new AsyncMethodLevelBefore(m).Run(instance));
            }
        }

        protected abstract Func<Type, MethodInfo> GetMethodSelector(Conventions conventions);

        protected abstract Func<Type, MethodInfo> GetAsyncMethodSelector(Conventions conventions);

        protected virtual bool ReverseClassMethods()
        {
            return false;
        }

        protected abstract bool CanRun(nspec instance);

        protected abstract void RunHooks(nspec instance);

        public void Run(nspec instance)
        {
            if (CanRun(instance))
            {
                RunAndHandleException(RunHooks, instance, ref Exception);
            }
        }

        public static bool RunAndHandleException(Action<nspec> action, nspec instance, ref Exception exceptionToSet)
        {
            bool hasThrown = false;

            try
            {
                action(instance);
            }
            catch (TargetInvocationException invocationException)
            {
                if (exceptionToSet == null) exceptionToSet = instance.ExceptionToReturn(invocationException.InnerException);

                hasThrown = true;
            }
            catch (Exception exception)
            {
                if (exceptionToSet == null) exceptionToSet = instance.ExceptionToReturn(exception);

                hasThrown = true;
            }

            return hasThrown;
        }

        protected void RunClassHooks(nspec instance)
        {
            // class (method-level)

            if (ClassHook != null && AsyncClassHook != null)
            {
                throw new AsyncMismatchException(
                    $"A spec class with all its ancestors cannot set both sync and async " +
                    "class-level '{classHookName}' hooks, they should either be all sync or all async");
            }

            ClassHook.SafeInvoke(instance);

            AsyncClassHook.SafeInvoke(instance);
        }

        protected void RunContextHooks()
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

        protected void RecurseAncestors(Action<Context> ancestorAction)
        {
            if (context.Parent != null) ancestorAction(context.Parent);
        }

        protected static List<MethodInfo> GetMethodsFromHierarchy(
            List<Type> classHierarchy, Func<Type, MethodInfo> selectMethod)
        {
            return classHierarchy
                .Select(selectMethod)
                .Where(m => m != null)
                .ToList();
        }

        public HookChainBase(Context context, string hookName, string asyncHookName, string classHookName)
        {
            this.context = context;
            this.hookName = hookName;
            this.asyncHookName = asyncHookName;
            this.classHookName = classHookName;
        }

        public Action Hook;
        public Func<Task> AsyncHook;
        
        public Action<nspec> ClassHook { get; protected set; }
        public Action<nspec> AsyncClassHook { get; protected set; }

        public Exception Exception;

        protected readonly Context context;

        protected readonly string hookName;
        protected readonly string asyncHookName;
        protected readonly string classHookName;
    }
}
