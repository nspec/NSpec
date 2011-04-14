using NUnit.Framework;
using System.Linq;

namespace SampleSpecs.Compare.NUnit
{
    [TestFixture]
    public class given_dortitos_are_registered_in_A1_for_50_cents
    {
        private VendingMachine machine;

        [SetUp]
        public void setup()
        {
            machine = new VendingMachine();

            machine.RegisterItem("Doritos", "A1", .5m);
        }

        [Test]
        public void should_be_1_item_in_the_vending_machine()
        {
            machine.Items.Count().ShouldBe(1);
        }

        [Test]
        public void the_item_should_have_the_name_doritos()
        {
            machine.Items.First().Name.ShouldBe("Doritos");
        }

        [Test]
        public void the_item_should_have_a_slot_of_A1()
        {
            machine.Items.First().Slot.ShouldBe("A1");
        }

        [Test]
        public void the_item_should_have_a_cost_of_50_cents()
        {
            machine.Items.First().Price.ShouldBe(.5m);
        }
    }
}