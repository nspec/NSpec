using FluentAssertions;
using NSpec.Tests.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpec.Tests.describe_RunningSpecs.describe_before_and_after
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
                beforeAllAsync = async () => await Task.Run(() => sequence = "A");
                beforeAsync = async () => await Task.Run(() => sequence += "B");
                itAsync["spec 1"] = async () => await Task.Run(() => sequence += "1");
                itAsync["spec 2"] = async () => await Task.Run(() => sequence += "2"); //two specs cause before_each and after_each to run twice
                afterAsync = async () => await Task.Run(() => sequence += "C");
                afterAllAsync = async () => await Task.Run(() => sequence += "D");
            }
        }

        [Test]
        public void everything_async_runs_in_the_correct_order_and_with_the_correct_frequency()
        {
            Run(typeof(SpecClass));

            SpecClass.sequence.Should().Be("AB1CB2CD");
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class async_before_and_after_aliases : when_running_specs
    {
        class SpecClass : sequence_spec
        {
            void as_long_as_the_async_world_has_not_come_to_an_end()
            {
                beforeAllAsync = async () => await Task.Run(() => sequence = "A");
                beforeEachAsync = async () => await Task.Run(() => sequence += "B");
                itAsync["spec 1"] = async () => await Task.Run(() => sequence += "1");
                itAsync["spec 2"] = async () => await Task.Run(() => sequence += "2"); //two specs cause before_each and after_each to run twice
                afterEachAsync = async () => await Task.Run(() => sequence += "C");
                afterAllAsync = async () => await Task.Run(() => sequence += "D");
            }
        }

        [Test]
        public void everything_async_runs_in_the_correct_order_and_with_the_correct_frequency()
        {
            Run(typeof(SpecClass));

            SpecClass.sequence.Should().Be("AB1CB2CD");
        }
    }
}
