using System;
using System.Linq;
using NSpec;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_method_level_befores : when_running_specs
    {
        class SpecClass : nspec
        {
            public static Action MethodLevelBefore = () => { };
            public static Action SubContextBefore = () => { };

            void method_level_context()
            {
                before = MethodLevelBefore;

                context["sub context"] = () => 
                {
                    before = SubContextBefore;

                    it["needs an example or it gets filtered"] = todo;
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Init(typeof(SpecClass));

            Run();
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
