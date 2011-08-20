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

            var method = type.Methods().First(m => convention.IsMethodLevelContext(m.Name));

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