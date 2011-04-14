using System.Collections.Generic;
using System.Linq;

namespace SampleSpecs.Compare.NUnit
{
    public class VendingMachine
    {
        public VendingMachine()
        {
            items = new Dictionary<string, Item>();
        }

        public void RegisterItem(string slot, string name, decimal price)
        {
            if (items.ContainsKey(slot)) throw new SlotAlreadyTakenException();

            items.Add(slot, new Item() { Name = name, Slot = slot, Price = price });
        }

        Dictionary<string, Item> items;

        public IList<Item> Items()
        {
            return items.Values.ToList();
        }

        public void Clear(string slot)
        {
            items.Remove(slot);
        }
    }
}