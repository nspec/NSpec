using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.Domain
{
    public class AsyncMethodLevelBefore
    {
        public AsyncMethodLevelBefore(MethodInfo method)
        {
            this.method = method;
        }

        public void Run(nspec nspec)
        {
            if (method.ReturnType == typeof(void))
            {
                throw new ArgumentException("'async void' method-level befores are not supported, please use 'async Task' instead");
            }

            if (method.ReturnType.IsGenericType)
            {
                throw new ArgumentException("'async Task<T>' method-level befores are not supported, please use 'async Task' instead");
            }

            Func<Task> asyncWork = () => (Task)method.Invoke(nspec, null);

            asyncWork.Offload();
        }

        MethodInfo method;
    }
}
