using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    public class describe_it_behaviour_in_xcontext : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                xcontext["sub context"] = () =>
                {
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
        public void the_example_should_be_pending()
        {
            methodContext.Contexts.First().Examples.First().Pending.should_be(true);
        }
    }

    [TestFixture]
    public class describe_xcontext : when_running_specs
    {
        class SpecClass : nspec
        {
            public static string output = string.Empty;
            public static Action MethodLevelBefore = () => { throw new Exception("this should not run."); };
            public static Action SubContextBefore = () => { throw new Exception("this should not run."); };

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
        public void it_should_not_run_befores_on_pending_context()
        {
            methodContext.AllExamples().First().Exception.should_be(null);
        }
    }
}
