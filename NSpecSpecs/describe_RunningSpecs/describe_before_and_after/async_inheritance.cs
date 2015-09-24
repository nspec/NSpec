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

            async Task after_each()
            {
                await Task.Run(() => sequence += "F");
            }

            async Task after_all()
            {
                await Task.Run(() => sequence += "H");
            }
        }

        class DerivedClass : BaseSpec
        {
            void a_context()
            {
                beforeAllAsync = async () => await Task.Run(() => sequence += "B");

                beforeAsync = async () => await Task.Run(() => sequence += "D");
                specify = () => 1.Is(1);
                afterAsync = async () => await Task.Run(() => sequence += "E");

                afterAllAsync = async () => await Task.Run(() => sequence += "G");
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