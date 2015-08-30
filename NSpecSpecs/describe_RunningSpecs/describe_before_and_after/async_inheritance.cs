using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    [Category("Async")]
    public class async_inheritance : when_running_specs
    {
        class BaseSpec : sequence_spec
        {
            async Task before_all()
            {
                await Task.Run(() => sequence = "A");
            }

            async Task before_each()
            {
                await Task.Run(() => sequence += "C");
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

        class DerivedClass : BaseSpec
        {
            void a_context()
            {
                asyncBeforeAll = async () => await Task.Run(() => sequence += "B");

                asyncBefore = async () => await Task.Run(() => sequence += "D");
                specify = () => 1.Is(1); // TODO-ASYNC
                asyncAfter = async () => await Task.Run(() => sequence += "E");

                afterAll = () => sequence += "G"; // TODO-ASYNC
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(DerivedClass));
        }

        [Test]
        public void before_alls_at_every_level_run_before_before_eaches_from_the_outside_in()
        {
            DerivedClass.sequence.should_start_with("ABCD");
        }

        [Test]
        public void after_alls_at_every_level_run_after_after_eaches_from_the_inside_out()
        {
            DerivedClass.sequence.should_end_with("EFGH");
        }
    }
}