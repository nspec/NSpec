using System;
using System.Reflection;
using System.Threading.Tasks;

namespace NSpec.Domain
{
    public class AsyncExample : ExampleBase
    {
        public override void Run(nspec nspec)
        {
            asyncAction.Offload();
        }

        public override void RunPending(nspec nspec)
        {
            // don't run example body, as this example is being skipped;
            // and no consistency check to perform on passed example body
        }

        public override bool IsAsync
        {
            get { return true; }
        }

        public override MethodInfo BodyMethodInfo
        {
            get { return asyncAction.GetMethodInfo(); }
        }

        /* No need for the following:
         * Async lambda expressions cannot be converted to expression trees

        public AsyncExample(Expression<Func<Task>> asyncExpr, bool pending = false)
            : this(Parse(asyncExpr), null, asyncExpr.Compile(), pending) { }

         */

        public AsyncExample(string name = "", string tags = "", Func<Task> asyncAction = null, bool pending = false)
            : base(name, tags, pending)
        {
            this.asyncAction = asyncAction;
        }

        Func<Task> asyncAction;
    }
}
