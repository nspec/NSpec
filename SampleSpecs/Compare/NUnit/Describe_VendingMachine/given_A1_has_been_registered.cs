using NUnit.Framework;

namespace SampleSpecs.Compare.NUnit.Describe_VendingMachine
{
    [TestFixture]
    public class given_A1_has_been_registered
    {
        [SetUp]
        public void setup()
        {
            machine = new VendingMachine();

            machine.RegisterItem("A1","doritos",.5m);
        }

        [Test]
        public void should_not_be_InStock()
        {
            machine.InStock("A1").ShouldBeFalse();
        }

        [Test]
        public void given_A1_is_stocked_it_should_be_in_stock()
        {
            machine.Stock("A1", 16);

            machine.InStock("A1").ShouldBeTrue();
        }

        [Test, ExpectedException(typeof(NothingRegisteredException))]
        public void when_stocking_a_slot_that_has_nothing_registered_should_throw_NothingRegisteredException()
        {
            machine.Stock("A2",1);
        }

        private VendingMachine machine;
    }
}