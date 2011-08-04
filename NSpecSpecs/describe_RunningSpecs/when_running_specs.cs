using System;
using System.Linq;
using NSpec.Domain;
using NSpec.Domain.Extensions;
using NUnit.Framework;
using NSpec;

namespace NSpecSpecs.WhenRunningSpecs
{
    public class when_running_specs
    {
        [SetUp]
        public void setup_base()
        {
            convention = new DefaultConventions();

            convention.Initialize();
        }

        protected void Run(Type type)
        {
            classContext = new ClassContext(type, convention);

            var method = type.Methods().First().Name;

            Run(type, method);
        }

        protected void Run(Type type, string methodName)
        {
            classContext = new ClassContext(type, convention);            

            var method = type.Methods().Single(s => s.Name == methodName);

            methodContext = new MethodContext(method);

            classContext.AddContext(methodContext);

            classContext.Build();

            classContext.Run();
        }

        protected ClassContext classContext;
        private DefaultConventions convention;
        protected Context methodContext;
    }
}