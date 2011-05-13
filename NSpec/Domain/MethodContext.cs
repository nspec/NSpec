using System;
using System.Reflection;

namespace NSpec.Domain
{
    public class MethodContext : Context
    {
        public MethodContext(MethodInfo method) : base(method.Name, 0)
        {
            Method = method;
        }

        public override void Run()
        {
            base.Run();

            try
            {
                Method.Invoke(CreateNSpecInstance(), null);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception executing context: {0}".With(FullContext()));

                throw e;
            }
        }
    }
}