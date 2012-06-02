using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_class_level_afterAll : when_running_specs
    {
        class SpecClass : nspec
        {
            public static List<string> log;

            static SpecClass()
            {
                log = new List<string>();
            }

            void after_all()
            {
                log.Add("SpecClass after_all");
            }
        }

        class DerivedClass : SpecClass
        {
            void after_all()
            {
                log.Add("DerivedClass after_all");
            }

            void context_b()
            {
                beforeEach = () => log.Add("context_b beforeEach");
                it["works"] = () => log.Add("context_b it");
            }
        }

        [Test]
        public void after_alls_are_run_in_the_correct_order()
        {
            Run(typeof(DerivedClass));

            var specInstance = classContext.GetInstance() as DerivedClass;

            SpecClass.log.should_be(new[]
                {
                    "context_b beforeEach",
                    "context_b it",
                    "DerivedClass after_all",
                    "SpecClass after_all"
                });
        }
    }
}
