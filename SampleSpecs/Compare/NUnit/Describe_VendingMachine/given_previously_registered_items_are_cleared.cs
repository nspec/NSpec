using NUnit.Framework;
using System.Linq;

namespace SampleSpecs.Compare.NUnit.Describe_VendingMachine
{
    [TestFixture]
    public class given_previously_registered_items_are_cleared : DescribeVendingMachine
    {
        [SetUp]
        public void setup()
        {
            given_a_new_VendingMachine();

            given_doritos_are_registerd_in_A1_for_50_cents();

            machine.Clear("A1");
        }

        [Test]
        public void there_should_be_no_items_in_the_vending_machine()
        {
            machine.Items().Count().ShouldBe(0);
        }
    }
}