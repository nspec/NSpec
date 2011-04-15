using NUnit.Framework;
using System.Linq;

namespace SampleSpecs.Compare.NUnit.Describe_VendingMachine
{
    [TestFixture]
    public class given_two_items_are_registered : DescribeVendingMachine
    {
        [SetUp]
        public void setup()
        {
            given_a_new_VendingMachine();

            given_doritos_are_registerd_in_A1_for_50_cents();

            machine.RegisterItem("A2","mountain dew",.5m);
        }

        [Test]
        public void should_be_2_items_in_the_vending_machine()
        {
            machine.Items().Count().ShouldBe(2);
        }
    }
}