using System.Collections.Generic;

namespace SampleSpecs.Compare.NUnit
{
    public class VendingMachine
    {
        public VendingMachine()
        {
            Items = new List<Item>();
        }

        public void RegisterItem(string name, string slot, decimal price)
        {
            Items.Add(new Item() { Name = name, Slot = slot, Price = price });
        }

        public IList<Item> Items { get; set; }
    }
}