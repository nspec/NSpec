using System;
using System.Linq;
using NSpec.Domain;
using NSpec.Domain.Extensions;

namespace NSpecSpecs.WhenRunningSpecs
{
    public class when_running_specs
    {
        protected void Run(Type type)
        {
            classContext = new ClassContext(type);

            var method = type.Methods().First();

            methodContext = new MethodContext(method);

            classContext.AddContext(methodContext);

            classContext.Run();
        }

        protected Context classContext;
        protected Context methodContext;
    }
}