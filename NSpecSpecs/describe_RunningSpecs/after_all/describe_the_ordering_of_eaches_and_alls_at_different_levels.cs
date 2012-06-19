using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.after_all
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_the_ordering_of_eaches_and_alls_at_different_levels : when_running_specs
    {
        class SpecClass : SequenceSpec
        {
            void before_all()
            {
                sequence = "A";
            }

            void before_each()
            {
                sequence += "C";
            }

            void as_long_as_the_world_has_not_come_to_an_end()
            {
                beforeAll = () => sequence += "B";

                before = () => sequence += "D";
                specify = () => 1.Is(1);
                specify = () => 2.Is(2); //two specs cause before_each and after_each to run twice
                after = () => sequence += "E";

                afterAll = () => sequence += "G";
            }

            void after_each()
            {
                sequence += "F";
            }

            void after_all()
            {
                sequence += "H";
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void before_alls_at_every_level_run_before_before_eaches_from_the_outside_in()
        {
            SpecClass.sequence.should_start_with("ABCD");
        }

        [Test]
        public void after_alls_at_every_level_run_after_after_eaches_from_the_inside_out()
        {
            SpecClass.sequence.should_end_with("EFGH");
        }
    }
}
