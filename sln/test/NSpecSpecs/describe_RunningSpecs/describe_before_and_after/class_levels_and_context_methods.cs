using FluentAssertions;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class class_levels_and_context_methods : when_running_specs
    {
        class SpecClass : sequence_spec
        {
            void before_all()
            {
                sequence = "A";
            }

            void before_each()
            {
                sequence += "C";
            }

            void a_context()
            {
                beforeAll = () => sequence += "B";

                before = () => sequence += "D";
                specify = () => 1.Should().Be(1, String.Empty);
                after = () => sequence += "E";

                afterAll = () => sequence += "G";
            }

            void after_each()
            {
                sequence += "F";
            }

            void after_all()
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
            SpecClass.sequence.Should().StartWith("ABCD");
        }

        [Test]
        public void after_alls_at_every_level_run_after_after_eaches_from_the_inside_out()
        {
            SpecClass.sequence.Should().EndWith("EFGH");
        }
    }
}
