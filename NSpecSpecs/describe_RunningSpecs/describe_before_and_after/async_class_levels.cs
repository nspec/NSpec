using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class async_class_levels : when_running_specs
    {
        class SpecClass : sequence_spec
        {
            async Task before_all()
            {
                await Task.Run(() => sequence = "A");
            }

            async Task before_each()
            {
                await Task.Run(() => sequence += "B");
            }

            async Task it_one_is_one()
            {
                await Task.Run(() => sequence += "1");
            }

            async Task it_two_is_two()
            {
                await Task.Run(() => sequence += "2"); //two specs cause before_each and after_each to run twice
            }

            async Task after_each()
            {
                await Task.Run(() => sequence += "C");
            }

            void after_all() // TODO-ASYNC
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