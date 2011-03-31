using System;

internal class Account
{
    public decimal Balance { get; set; }

    public bool CanWithdraw(int amount)
    {
        return amount <= Balance;
    }

    public void Withdraw(int amount)
    {
        if (amount < 0) throw new InvalidOperationException();
    }
}