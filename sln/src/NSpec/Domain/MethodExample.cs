using System.Reflection;

namespace NSpec.Domain
{
    public class MethodExample : MethodExampleBase
    {
        public MethodExample(MethodInfo method, string tags)
            : base(method, tags)
        {
        }

        public override void Run(nspec nspec)
        {
            method.Invoke(nspec, null);
        }
    }
}
