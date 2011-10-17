using System;
using System.Reflection;

namespace NSpec.Domain
{
    public class MethodContext : Context
    {
        public MethodContext(MethodInfo method)
            : base(method.Name, null, 0)
        {
            this.method = method;
        }

        public override void Build(nspec instance)
        {
            base.Build(instance);

            try
            {
                method.Invoke(instance, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception executing context: {0}".With(FullContext()));

                throw e;
            }
        }

        private MethodInfo method;
    }
}