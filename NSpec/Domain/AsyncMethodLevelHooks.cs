using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.Domain
{
    public abstract class AsyncMethodLevelHook
    {
        public AsyncMethodLevelHook(MethodInfo method, string hookName)
        {
            this.method = method;
            this.hookName = hookName;
        }

        public void Run(nspec nspec)
        {
            if (method.ReturnType == typeof(void))
            {
                throw new ArgumentException("'async void' method-level {0} is not supported, please use 'async Task' instead", hookName);
            }

            if (method.ReturnType.IsGenericType)
            {
                throw new ArgumentException("'async Task<T>' method-level {0} is not supported, please use 'async Task' instead", hookName);
            }

            Func<Task> asyncWork = () => (Task)method.Invoke(nspec, null);

            asyncWork.Offload();
        }

        readonly MethodInfo method;
        readonly string hookName;
    }

    public class AsyncMethodLevelBefore : AsyncMethodLevelHook
    {
        public AsyncMethodLevelBefore(MethodInfo method) : base(method, "before_each") { }
    }

    public class AsyncMethodLevelBeforeAll : AsyncMethodLevelHook
    {
        public AsyncMethodLevelBeforeAll(MethodInfo method) : base(method, "before_all") { }
    }

    public class AsyncMethodLevelAfter : AsyncMethodLevelHook
    {
        public AsyncMethodLevelAfter(MethodInfo method) : base(method, "after_each") { }
    }
}
