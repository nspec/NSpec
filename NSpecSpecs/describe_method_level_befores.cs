using System;
using System.Linq;
using NUnit.Framework;
using NSpec;

namespace NSpecSpecs
{
    [TestFixture]
    public class describe_method_level_befores : when_running_specs
    {
        private class SpecClass : nspec
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
            Run(typeof(SpecClass));
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
    }
}
