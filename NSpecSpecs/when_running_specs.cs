using System;
using System.Linq;
using System.Reflection;
using NSpec.Domain;
using NSpec.Domain.Extensions;

namespace NSpecSpecs
{
    public class when_running_specs
    {
        protected void Run(Type type)
        {
            classContext = new Context(type);

            var method = Enumerable.First<MethodInfo>(type.Methods());

            methodContext = new Context(method);

            classContext.AddContext(methodContext);

            classContext.Run();
        }

        protected Context classContext;
        protected Context methodContext;
    }
}