using System;
using System.Linq;
using NSpec;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_method_level_afters : when_running_specs
    {
        class SpecClass : nspec
        {
            public static Action MethodLevelAfter = () => { };
            public static Action SubContextAfter = () => { };

            void method_level_context()
            {
                after = MethodLevelAfter;

                context["sub context"] = () => 
                {
                    after = SubContextAfter;

                    it["needs an example or it gets filtered"] = todo;
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void it_should_set_method_level_after()
        {
            methodContext.After.should_be(SpecClass.MethodLevelAfter);
        }

        [Test]
        public void it_should_set_after_on_sub_context()
        {
            methodContext.Contexts.First().After.should_be(SpecClass.SubContextAfter);
        }
    }
}
