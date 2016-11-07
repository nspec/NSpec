using FluentAssertions;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class async_middle_abstract : when_running_specs
    {
        class Base : sequence_spec
        {
            async Task before_all()
            {
                await Task.Run(() => sequence += "A");
            }
            async Task after_all()
            {
                await Task.Run(() => sequence += "F");
            }
        }

        abstract class Abstract : Base
        {
            async Task before_all()
            {
                await Task.Run(() => sequence += "B");
            }
            async Task before_each()
            {
                await Task.Run(() => sequence += "C");
            }
            async Task after_each()
            {
                await Task.Run(() => sequence += "D");
            }
            async Task after_all()
            {
                await Task.Run(() => sequence += "E");
            }
        }

        class Concrete : Abstract
        {
            async Task it_one_is_one()
            {
                await Task.Delay(0);

                1.Should().Be(1);
            }
        }

        [SetUp]
        public void setup()
        {
            Concrete.sequence = "";
        }

        [Test]
        public void befores_are_run_from_middle_abstract_classes()
        {
            Run(typeof(Concrete));

            Concrete.sequence.Should().StartWith("ABC");
        }

        [Test]
        public void afters_are_run_from_middle_abstract_classes()
        {
            Run(typeof(Concrete));

            Concrete.sequence.Should().EndWith("DEF");
        }
    }
}
