using NUnit.Framework;
using System.Linq;

namespace SampleSpecs.Compare.NUnit
{
    [TestFixture]
    public class given_dortitos_are_registered_in_A1_for_50_cents 
    {
        [SetUp]
        public void setup()
        {
            machine = new VendingMachine();

            machine.RegisterItem("A1", "Doritos", .5m);
        }
        protected VendingMachine machine;
    }

    [TestFixture]
    public class Describe_RegisterItem : given_dortitos_are_registered_in_A1_for_50_cents
    {
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

    [TestFixture]
    public class Describe_Clear : given_dortitos_are_registered_in_A1_for_50_cents
    {
        [SetUp]
        public void after_clearing_slot_A1()
        {
            machine.Clear("A1");
        }

        [Test]
        public void there_should_be_no_items_in_the_vending_machine()
        {
            machine.Items().Count().ShouldBe(0);
        }
    }
}