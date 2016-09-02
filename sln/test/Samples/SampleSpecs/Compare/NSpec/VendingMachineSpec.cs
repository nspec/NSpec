using System;
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

            specify = ()=> machine.Items().should_be_empty();

            it["getting item A1 should throw ItemNotRegistered"] = expect<ItemNotRegisteredException>(() => machine.Item("A1"));

            context["given doritos are registered in A1 for 50 cents"] = () =>
            {
                before = () => machine.RegisterItem("A1", "doritos", .5m);

                specify = () => machine.Items().Count().should_be(1);

                specify = () => machine.Item("A1").Name.should_be("doritos");

                specify = () => machine.Item("A1").Price.should_be(.5m);

                context["given a second item is registered"] = () =>
                {
                    before = () => machine.RegisterItem("A2", "mountain dew", .6m);

                    specify = () => machine.Items().Count().should_be(2);
                };
            };
            //got to force/refactor getting rid of the dictionary soon
        }
        private VendingMachine machine;
    }

    public class ItemNotRegisteredException : Exception
    {
    }

    internal class VendingMachine
    {
        public VendingMachine()
        {
            items = new List<Item>();
        }

        public IEnumerable<Item> Items()
        {
            return items;
        }

        public void RegisterItem(string slot, string name, decimal price)
        {
            items.Add(new Item{Name = name,Price = price,Slot = slot});
        }

        public Item Item(string slot)
        {
            if (!items.Any(i => i.Slot == slot)) throw new ItemNotRegisteredException();

            return items.First();
        }
        private List<Item> items;
    }

    internal class Item
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Slot { get; set; }
    }
}