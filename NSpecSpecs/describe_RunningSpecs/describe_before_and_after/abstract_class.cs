using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    public class abstract_class : when_running_specs
    {
        abstract class Abstract : sequence_spec
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
                specify = () => 1.Is(1);
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
        class Concrete : Abstract {}

        [Test]
        public void all_features_are_supported_from_abstract_classes_when_run_under_the_context_of_a_derived_concrete()
        {
            Run(typeof(Concrete));
            Concrete.sequence.Is("ABCDEFGH");
        }
    }
}