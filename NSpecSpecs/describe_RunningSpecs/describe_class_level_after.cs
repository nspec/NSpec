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
                sequence += "E";
            }
            void after_each()
            {
                sequence += "D";
            }
            void running_example()
            {
                it["example1"] = () => sequence += "1";
            }
        }

        abstract class DerivedClass1 : BaseClass
        {
            void after_each()
            {
                sequence += "C";
            }
        }

        abstract class DerivedClass2 : DerivedClass1 {}

        abstract class DerivedClass3 : DerivedClass2
        {
            void after_each()
            {
                sequence += "B";
            }
        }

        class DerivedClass5 : DerivedClass3
        {
            void after_each()
            {
                sequence += "A";
            }

            void running_example()
            {
                it["example2"] = () => sequence += "2";
            }
        }

        [Test]
        public void filter_by_tags_for_only_DerivedClass5()
        {
            DerivedClass5.sequence = "";

            tags = typeof(DerivedClass5).Name;

            Run(typeof(DerivedClass5));

            DerivedClass5.sequence.Is("2ABCDE");
        }

        //[Test,Ignore("intermittent every other failure with ncrunch")]
        [Test]
        public void without_filtering()
        {
            DerivedClass5.sequence = "";

            Run(typeof(DerivedClass5));

            //When the tags filter is not present it causes A and B run more than expected.
            //this is because both "running examples" run one from DerivedClass5 and one from 
            //DerivedClass1. And since A and B are after each, they run for each example

            DerivedClass5.sequence.Is("1D2ABCDE");
        }
    }
}
