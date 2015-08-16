using System.Reflection;

namespace NSpec.Domain
{
    public class MethodExample : ExampleBase
    {
        public MethodExample(MethodInfo method, string tags = null) : base(method.Name.Replace("_", " "), tags)
        {
            this.method = method;
        }

        public override void Run(nspec nspec)
        {
            method.Invoke(nspec, null);
        }

        MethodInfo method;
    }
}
