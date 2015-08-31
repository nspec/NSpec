using System;
using System.Reflection;
using System.Threading.Tasks;

namespace NSpec.Domain
{
    public class AsyncMethodExample : MethodExampleBase
    {
        public AsyncMethodExample(MethodInfo method, string tags)
            : base(method, tags)
        {
            runner = new AsyncMethodRunner(method, "example");
        }

        public override void Run(nspec nspec)
        {
            runner.Run(nspec);
        }

        readonly AsyncMethodRunner runner;
    }

    public class AsyncMethodRunner
    {
        public AsyncMethodRunner(MethodInfo method, string hookName)
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
}
