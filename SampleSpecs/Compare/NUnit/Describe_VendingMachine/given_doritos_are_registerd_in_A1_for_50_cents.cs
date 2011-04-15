using NUnit.Framework;
using System.Linq;

namespace SampleSpecs.Compare.NUnit.Describe_VendingMachine
{
    [TestFixture]
    public class given_doritos_are_registerd_in_A1_for_50_cents : DescribeVendingMachine
    {
        [SetUp]
        public void setup()
        {
            given_a_new_VendingMachine();

            given_doritos_are_registerd_in_A1_for_50_cents();
        }

        [Test]
        public void should_not_be_InStock()
        {
            machine.InStock("A1").ShouldBeFalse();
        }
        
        [Test]
        public void should_be_1_item_in_the_vending_machine()
        {
            machine.Items().Count().ShouldBe(1);
        }

        [Test]
        public void the_item_should_have_the_name_doritos()
        {
            TheFirstItem().Name.ShouldBe("Doritos");
        }

        [Test]
        public void the_item_should_have_a_slot_of_A1()
        {
            TheFirstItem().Slot.ShouldBe("A1");
        }

        [Test]
        public void the_item_should_have_a_cost_of_50_cents()
        {
            TheFirstItem().Price.ShouldBe(.5m);
        }

        [Test,ExpectedException(typeof(SlotAlreadyTakenException))]
        public void registering_cheetos_in_the_same_slot_should_throw_SlotAlreadyAssignedException()
        {
            machine.RegisterItem("A1", "cheetos", .5m);
        }

        private Item TheFirstItem()
        {
            return machine.Items().First();
        }
    }
}