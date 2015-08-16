using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpecSpecs
{
    public static class Utils
    {
        public static async Task RunActionAsync(Action action)
        {
            Task fictiousAsyncOperation = Task.Run(action);

            await fictiousAsyncOperation;
        }
    }
}
