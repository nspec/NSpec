using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NSpec.Domain
{
    public abstract class HookChainBase
    {
        public abstract void BuildMethodLevel(Conventions conventions, List<Type> classHierarchy);

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

        public HookChainBase(Context context)
        {
            this.context = context;
        }

        public Action Hook;
        public Func<Task> AsyncHook;
        
        public Action<nspec> ClassHook { get; protected set; }
        public Action<nspec> AsyncClassHook { get; protected set; }

        public Exception Exception;

        protected readonly Context context;
    }
}
