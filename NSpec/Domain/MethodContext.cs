using System.Reflection;

namespace NSpec.Domain
{
    public class MethodContext : Context
    {
        public MethodContext(MethodInfo method) : base(method.Name,0)
        {
            Method = method;
        }
    }
}