using System;
using System.Linq;
using System.Reflection;
using NSpec.Assertions;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;
using NSpec.Extensions;

namespace NSpecNUnit
{
    [TestFixture]
    public class describe_method_level_befores
    {
        private class SpecClass : spec
        {
            public static Action MethodLevelBefore = () => { };
            public static Action SubContextBefore = () => { };

            public void method_level_context()
            {
                before = MethodLevelBefore;

                context["sub context"] = () => 
                {
                    before = SubContextBefore;
                };
            }
        }

        [SetUp]
        public void setup()
        {
            classContext = new Context(typeof(SpecClass));

            var method = typeof(SpecClass).Methods().Single(m=>m.Name=="method_level_context");

            methodContext = new Context(method);

            classContext.AddContext( methodContext);

            classContext.Run();
        }

        [Test]
        public void it_should_set_method_level_before()
        {
            methodContext.Before.should_be(SpecClass.MethodLevelBefore);
        }

        [Test]
        public void it_should_set_before_on_sub_context()
        {
            methodContext.Contexts.First().Before.should_be(SpecClass.SubContextBefore);
        }

        private Context classContext;
        private Context methodContext;
    }
}
