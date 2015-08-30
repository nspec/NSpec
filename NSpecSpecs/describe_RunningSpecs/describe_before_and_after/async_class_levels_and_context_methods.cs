using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class async_class_levels_and_context_methods : when_running_specs
    {
        class SpecClass : sequence_spec
        {
            async Task before_all()
            {
                await Task.Run(() => sequence = "A");
            }

            async Task before_each()
            {
                await Task.Run(() => sequence += "C");
            }

            void a_context()
            {
                asyncBeforeAll = async () => await Task.Run(() => sequence += "B");

                asyncBefore = async () => await Task.Run(() => sequence += "D");
                specify = () => 1.Is(1); // TODO-ASYNC
                asyncAfter = async () => await Task.Run(() => sequence += "E");

                asyncAfterAll = async () => await Task.Run(() => sequence += "G");
            }

            void after_each() // TODO-ASYNC
            {
                sequence += "F";
            }

            void after_all() // TODO-ASYNC
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
