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
        }

        // TODO-ASYNC extract common Run logic from AsyncMethodExample and AsyncMethodLevelHook

        public override void Run(nspec nspec)
        {
            if (method.ReturnType == typeof(void))
            {
                throw new ArgumentException("'async void' method-level specifications are not supported, please use 'async Task' instead");
            }

            if (method.ReturnType.IsGenericType)
            {
                throw new ArgumentException("'async Task<T>' method-level specifications are not supported, please use 'async Task' instead");
            }

            Func<Task> asyncWork = () => (Task)method.Invoke(nspec, null);

            asyncWork.Offload();
        }
    }
}
