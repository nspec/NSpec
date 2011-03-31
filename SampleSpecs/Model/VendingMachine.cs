using System.Collections.Generic;

public class VendingMachine
{
    private Dictionary<string, int> _inventory;
    private Dictionary<string, double> _pricePoint;

    public VendingMachine()
    {
        _inventory = new Dictionary<string, int>();
        _pricePoint = new Dictionary<string, double>();
    }

    public void AddInventory(string item)
    {
        if (_inventory.ContainsKey(item) == false) _inventory.Add(item, 0);

        _inventory[item] += 1;
    }

    public void Buy(string item)
    {
        _inventory[item] -= 1;
        _cash += _pricePoint[item];
    }

    public int Inventory(string item)
    {
        return _inventory[item];
    }

    private double _cash;
    public double Cash
    {
        get
        {
            return _cash;
        }
    }

    public void PricePoint(string item, double amount)
    {
        _pricePoint.Add(item, amount);
    }
}