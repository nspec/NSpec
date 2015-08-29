using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class async_before_and_after : when_running_specs
    {
        class SpecClass : sequence_spec
        {
            void as_long_as_the_async_world_has_not_come_to_an_end()
            {
                asyncBeforeAll = async () => await Task.Run(() => sequence = "A");
                asyncBefore = async () => await Task.Run(() => sequence += "B");
                asyncIt["spec 1"] = async () => await Task.Run(() => sequence += "1");
                asyncIt["spec 2"] = async () => await Task.Run(() => sequence += "2"); //two specs cause before_each and after_each to run twice
                after = async () => await Task.Run(() => sequence += "C"); // TODO-ASYNC 
                afterAll = async () => await Task.Run(() => sequence += "D"); // TODO-ASYNC 
            }
        }

        [Test]
        public void everything_async_runs_in_the_correct_order_and_with_the_correct_frequency()
        {
            Run(typeof(SpecClass));

            SpecClass.sequence.Is("AB1CB2CD");
        }
    }
}
