using System.Collections.Generic;
using System.Linq;
using NSpec;

namespace SampleSpecs.Compare.NSpec
{
    class VendingMachineSpec : nspec
    {
        void given_new_vending_machine()
        {
            before = () => machine = new VendingMachine();

            it["should have no items"] = ()=> machine.Items().should_be_empty();

            context["given doritos are registered in A1 for 50 cents"] = () =>
            {
                before = () => machine.RegisterItem("A1", "doritos", .5m);

                specify = () => machine.Items().Count().should_be(1);
            };
        }
        private VendingMachine machine;
    }

    internal class VendingMachine
    {
        public VendingMachine()
        {
            items = new Item[] { };
        }

        public IEnumerable<Item> Items()
        {
            return items;
        }

        public void RegisterItem(string slot, string name, decimal price)
        {
            items = new[]{new Item()};
        }
        private Item[] items;
    }

    internal class Item
    {
    }
}