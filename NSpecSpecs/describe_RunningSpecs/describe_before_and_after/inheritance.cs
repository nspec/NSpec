using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using NSpec.Assertions.nUnit;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    class BaseSpec : sequence_spec
    {
        void before_all()
        {
            sequence = "A";
        }

        void before_each()
        {
            sequence += "C";
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

    class DerivedClass : BaseSpec
    {
        void a_context()
        {
            beforeAll = () => sequence += "B";

            before = () => sequence += "D";
            specify = () => 1.Is(1);
            after = () => sequence += "E";

            afterAll = () => sequence += "G";
        }
    }

    [TestFixture]
    public class inheritance : when_running_specs
    {
        [SetUp]
        public void setup()
        {
            Run(typeof(DerivedClass));
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