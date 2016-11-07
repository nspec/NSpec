using FluentAssertions;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System;
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
                beforeAllAsync = async () => await Task.Run(() => sequence += "A");
                beforeAsync = async () => await Task.Run(() => sequence += "C");

                context["a subcontext"] = () =>
                {
                    beforeAllAsync = async () => await Task.Run(() => sequence += "B");
                    beforeAsync = async () => await Task.Run(() => sequence += "D");

                    specify = () => 1.Should().Be(1, String.Empty);

                    afterAsync = async () => await Task.Run(() => sequence += "E");
                    afterAllAsync = async () => await Task.Run(() => sequence += "G");
                };

                afterAsync = async () => await Task.Run(() => sequence += "F");
                afterAllAsync = async () => await Task.Run(() => sequence += "H");
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
            SpecClass.sequence.Should().StartWith("ABCD");
        }

        [Test]
        public void after_alls_at_every_level_run_after_after_eaches_from_the_inside_out()
        {
            SpecClass.sequence.Should().EndWith("EFGH");
        }
    }
}