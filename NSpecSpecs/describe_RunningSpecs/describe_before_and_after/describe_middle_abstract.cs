using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_middle_abstract : when_running_specs
    {
        class BaseClass : sequence_spec
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

        abstract class MiddleAbstractClass : BaseClass
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
        class ConcreteClass : MiddleAbstractClass
        {
            void it_one_is_one()
            {
                1.Is(1);
            }
        }

        [SetUp]
        public void setup()
        {
            ConcreteClass.sequence = "";
        }

        [Test]
        public void befores_are_run_from_middle_abstract_classes()
        {
            Run(typeof(ConcreteClass));

            ConcreteClass.sequence.should_start_with("ABC");
        }

        [Test]
        public void afters_are_run_from_middle_abstract_classes()
        {
            Run(typeof(ConcreteClass));

            ConcreteClass.sequence.should_end_with("DEF");
        }
    }
}
