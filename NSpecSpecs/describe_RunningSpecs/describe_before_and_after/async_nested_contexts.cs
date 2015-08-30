using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    [Category("Async")]
    public class async_nested_contexts : when_running_specs
    {
        class SpecClass : sequence_spec
        {
            void a_context()
            {
                asyncBeforeAll = async () => await Task.Run(() => sequence += "A");
                asyncBefore = async () => await Task.Run(() => sequence += "C");

                context["a subcontext"] = () =>
                {
                    asyncBeforeAll = async () => await Task.Run(() => sequence += "B");
                    asyncBefore = async () => await Task.Run(() => sequence += "D");

                    specify = () => 1.Is(1); // TODO-ASYNC

                    asyncAfter = async () => await Task.Run(() => sequence += "E");
                    afterAll = () => sequence += "G"; // TODO-ASYNC
                };

                asyncAfter = async () => await Task.Run(() => sequence += "F");
                afterAll = () => sequence += "H"; // TODO-ASYNC
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