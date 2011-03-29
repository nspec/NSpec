internal class Account
{
    public decimal Balance{get;set;}

    public bool CanWithdraw(int amount)
    {
        return amount <= Balance;
    }
}