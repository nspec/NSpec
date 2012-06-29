using NSpec;
using NSpecSpecs.describe_RunningSpecs.describe_before_and_after;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_class_level_after : when_running_specs
    {
        class BaseClass : sequence_spec
        {
            void after_all()
            {
                sequence += "C";
            }
            void after_each()
            {
                sequence += "B";
            }
        }

        class DerivedClass1 : BaseClass
        {
            void after_each()
            {
                sequence += "A";
            }

            void running_example()
            {
                it["works"] = () => sequence += "1";
            }
        }

        abstract class DerivedClass2 : DerivedClass1
        {
            void after_each()
            {
                sequence += "9";
            }
        }

        abstract class DerivedClass3 : DerivedClass2 {}

        abstract class DerivedClass4 : DerivedClass3
        {
            void after_each()
            {
                sequence += "8";
            }
        }

        class DerivedClass5 : DerivedClass4
        {
            void after_each()
            {
                sequence += "7";
            }

            void running_example()
            {
                it["works"] = () => sequence += "2";
            }
        }

        [Test]
        public void afters_are_run_in_the_correct_order()
        {
            DerivedClass1.sequence = "";

            Run(typeof(DerivedClass1));
            
            DerivedClass1.sequence.Is("1ABC");
        }

        [Test]
        public void filtering_for_only_DerivedClass5()
        {
            DerivedClass5.sequence = "";

            tags = typeof(DerivedClass5).Name;

            Run(typeof(DerivedClass5));

            DerivedClass5.sequence.Is("2789ABC");
        }

        [Test,Ignore("intermittent every other failure with ncrunch")]
        public void without_filtering()
        {
            DerivedClass5.sequence = "";

            Run(typeof(DerivedClass5));

            //you might expect A and B run just once
            //DerivedClass5.sequence.Is("789AB");

            //but when the tags filter is not in play it causes A and B run more than expected.
            //this is because both "running examples" run one from DerivedClass5 and one from 
            //DerivedClass1. And since A and B are after each, they run for each example

            DerivedClass5.sequence.Is("1AB2789ABC");
        }
    }
}
