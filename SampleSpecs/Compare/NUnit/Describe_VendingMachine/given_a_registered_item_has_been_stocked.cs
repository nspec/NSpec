using NUnit.Framework;

namespace SampleSpecs.Compare.NUnit.Describe_VendingMachine
{
    [TestFixture]
    public class given_a_registered_item_has_been_stocked : DescribeVendingMachine
    {
        [SetUp]
        public void setup()
        {
            given_a_new_VendingMachine();

            given_doritos_are_registerd_in_A1_for_50_cents();

            machine.Stock("A1", 16);
        }

        [Test]
        public void given_A1_is_stocked_it_should_be_in_stock()
        {
            machine.InStock("A1").ShouldBeTrue();
        }
    }
}