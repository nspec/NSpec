using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpecSpecs.WhenRunningSpecs;
using NSpec;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_class_level_after : when_running_specs
    {
        class SpecClass : nspec
        {
            public string ExecutionSequence;

            void after_each()
            {
                ExecutionSequence += "B";
            }
        }

        class DerivedClass : SpecClass
        {
            void after_each()
            {
                ExecutionSequence = "A";
            }

            void running_example()
            {
                it["works"] = () => ExecutionSequence.should_be(null);
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(DerivedClass));
        }

        //[Test]
        //public void afters_are_run_in_the_correct_order()
        //{
        //    (classContext.Contexts.First().GetInstance() as DerivedClass).ExecutionSequence.should_be("A");

        //    classContext.Contexts.Failures().Count().should_be(0);
        //}
    }
}
