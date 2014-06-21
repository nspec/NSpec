using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using NSpec.Assertions.nUnit;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    public class nested_contexts : when_running_specs
    {
        class SpecClass : sequence_spec
        {
            void a_context()
            {
                beforeAll = () => sequence = "A";
                before = () => sequence += "C";

                context["a subcontext"] = () =>
                {
                    beforeAll = () => sequence += "B";
                    before = () => sequence += "D";

                    specify = () => 1.Is(1);

                    after = () => sequence += "E";
                    afterAll = () => sequence += "G";
                };

                after = () => sequence += "F";
                afterAll = () => sequence += "H";
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