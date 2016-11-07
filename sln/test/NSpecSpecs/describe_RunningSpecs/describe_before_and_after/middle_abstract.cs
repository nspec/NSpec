using FluentAssertions;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class middle_abstract : when_running_specs
    {
        class Base : sequence_spec
        {
            void before_all()
            {
                sequence += "A";
            }
            void after_all()
            {
                sequence += "F";
            }
        }

        abstract class Abstract : Base
        {
            void before_all()
            {
                sequence += "B";
            }
            void before_each()
            {
                sequence += "C";
            }
            void after_each()
            {
                sequence += "D";
            }
            void after_all()
            {
                sequence += "E";
            }
        }

        class Concrete : Abstract
        {
            void it_one_is_one()
            {
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
