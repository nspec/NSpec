using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using NSpec.Assertions.nUnit;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    public class when_there_are_no_specs : when_running_specs
    {
        [SetUp]
        public void setup()
        {
            sequence_spec.sequence = "";
        }

        class before_all_example_spec : sequence_spec
        {
            void before_all()
            {
                sequence = "executed";
            }
        }

        [Test]
        public void before_all_is_not_executed()
        {
            Run(typeof (before_all_example_spec));

            sequence_spec.sequence.Is("");
        }

        class before_each_example_spec : sequence_spec
        {
            void before_each()
            {
                sequence = "executed";
            }
        }

        [Test]
        public void before_each_is_not_executed()
        {
            Run(typeof (before_each_example_spec));

            sequence_spec.sequence.Is("");
        }

        class after_each_example_spec : sequence_spec
        {
            void after_each()
            {
                sequence = "executed";
            }
        }

        [Test]
        public void after_each_is_not_executed()
        {
            Run(typeof (after_each_example_spec));

            sequence_spec.sequence.Is("");
        }

        class after_all_example_spec : sequence_spec
        {
            void after_all()
            {
                sequence = "executed";
            }
        }

        [Test]
        public void after_all_is_not_executed()
        {
            Run(typeof (after_all_example_spec));

            sequence_spec.sequence.Is("");
        }
    }
}