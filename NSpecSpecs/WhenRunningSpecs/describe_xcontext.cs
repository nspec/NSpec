using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    class describe_xcontext : when_running_specs
    {
        class SpecClass : nspec
        {
            public static Action MethodLevelBefore = () => { };
            public static Action SubContextBefore = () => { };

            void method_level_context()
            {
                before = MethodLevelBefore;

                xcontext["sub context"] = () =>
                {
                    before = SubContextBefore;

                    it["needs an example or it gets filtered"] =
                        () => "Hello World".should_be("Hello World");
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
            methodContext.Contexts.First().Examples.First().Pending.should_be(true);
        }
    }
}
