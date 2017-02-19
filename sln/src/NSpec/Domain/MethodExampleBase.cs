using NSpec.Domain.Extensions;
using System.Reflection;

namespace NSpec.Domain
{
    public abstract class MethodExampleBase : ExampleBase
    {
        public MethodExampleBase(MethodInfo method, string tags)
            : base(method.Name.Replace("_", " "), tags)
        {
            this.method = method;
        }

        public override void RunPending(nspec nspec)
        {
            // don't run example body, as this example is being skipped;
            // and no consistency check to perform on passed example body
        }

        public override bool IsAsync
        {
            get { return method.IsAsync(); }
        }

        public override MethodInfo BodyMethodInfo
        {
            get { return method; }
        }

        protected MethodInfo method;
    }
}
