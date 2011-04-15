using NUnit.Framework;

namespace SampleSpecs.Compare.NUnit
{
    [TestFixture]
    public class Describe_StockItem
    {
        [Test]
        public void given_A1_is_registered_and_stocked_it_should_be_in_stock()
        {
            var machine = new VendingMachine();

            machine.RegisterItem("A1","doritos",.5m);

            machine.Stock("A1", 16);

            machine.InStock("A1").ShouldBeTrue();
        }
    }
}