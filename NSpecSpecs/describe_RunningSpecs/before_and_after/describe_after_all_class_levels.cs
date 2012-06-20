using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.before_and_after
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_class_level_after_all : when_running_specs
    {
        class SpecClass : SequenceSpec
        {
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
                sequence += "1";
            }

            void it_two_is_two()
            {
                sequence += "2"; //two specs cause before_each and after_each to run twice
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

        [Test]
        public void everything_runs_in_the_correct_order()
        {
            Run(typeof(SpecClass));

            SpecClass.sequence.Is("AB1CB2CD");
        }
    }
}