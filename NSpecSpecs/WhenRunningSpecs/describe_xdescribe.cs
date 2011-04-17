using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    public class describe_xdescribe : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                xdescribe["sub context"] = () =>
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
}
