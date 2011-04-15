using System.Collections.Generic;
using NSpec;

namespace SampleSpecs.Compare.NSpec
{
    class VendingMachineSpec : nspec
    {
        void given_new_vending_machine()
        {
            before = () => machine = new VendingMachine();

            it["should have no items"] = ()=> machine.Items().should_be_empty();
        }
        private VendingMachine machine;
    }

    internal class VendingMachine
    {
        public IEnumerable<Item> Items()
        {
            return new Item[] { };
        }
    }

    internal class Item
    {
    }
}