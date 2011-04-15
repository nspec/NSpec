using NUnit.Framework;

namespace SampleSpecs.Compare.NUnit.Describe_VendingMachine
{
    [TestFixture]
    public class given_nothing_has_been_registered : DescribeVendingMachine
    {
        [SetUp]
        public void setup()
        {
            given_a_new_VendingMachine();
        }

        [Test, ExpectedException(typeof(NothingRegisteredException))]
        public void when_stocking_a_slot_should_throw_NothingRegisteredException()
        {
            machine.Stock("A1", 1);
        }
    }
}