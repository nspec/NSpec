using System.Collections.Generic;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.after_all
{
    class SpecClass : nspec
    {
        public static string sequence;

        void before_all()
        {
            sequence = "A";
        }

        void before_each()
        {
            sequence += "B";
        }

        void it_one_is_one()
        {
            1.Is(1);
        }

        void it_two_is_two()
        {
            1.Is(1);
        }

        void after_each()
        {
            sequence += "C";
        }

        void after_all()
        {
            sequence += "D";
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_after_all : when_running_specs
    {
        [Test]
        public void after_alls_are_run_in_the_correct_order()
        {
            Run(typeof(SpecClass));

            SpecClass.sequence.Is("ABCBCD");
            //SpecClass.log.should_be(new[]
            //    {
            //        "context_b beforeEach",
            //        "context_b it 1",
            //        "context_b afterEach",
            //        "DerivedClass after_each",
            //        "SpecClass after_each",
            //        "context_b beforeEach",
            //        "context_b it 2",
            //        "context_b afterEach",
            //        "DerivedClass after_each",
            //        "SpecClass after_each",
            //        "context_b afterAll",
            //        "DerivedClass after_all",
            //        "SpecClass after_all"
            //    });
        }

    }

    public class describe_after_all_class_levels{}
}
