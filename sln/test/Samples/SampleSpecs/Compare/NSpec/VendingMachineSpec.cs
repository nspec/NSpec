using System;
using System.Collections.Generic;
using System.Linq;
using NSpec;
using FluentAssertions;

namespace SampleSpecs.Compare.NSpec
{
    class VendingMachineSpec : nspec
    {
        void given_new_vending_machine()
        {
            before = () => machine = new VendingMachine();

            specify = () => machine.Items().Should().BeEmpty(String.Empty);

            it["getting item A1 should throw ItemNotRegistered"] = expect<ItemNotRegisteredException>(() => machine.Item("A1"));

            context["given doritos are registered in A1 for 50 cents"] = () =>
            {
                before = () => machine.RegisterItem("A1", "doritos", .5m);

                specify = () => machine.Items().Count().Should().Be(1, String.Empty);

                specify = () => machine.Item("A1").Name.Should().Be("doritos", String.Empty);

                specify = () => machine.Item("A1").Price.Should().Be(.5m, String.Empty);

                context["given a second item is registered"] = () =>
                {
                    before = () => machine.RegisterItem("A2", "mountain dew", .6m);

                    specify = () => machine.Items().Count().Should().Be(2, String.Empty);
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