using System.Collections.Generic;
using NSpec;

class describe_contexts : nspec
{
    void describe_Account()
    {
        //contexts can be nested n-deep and contain befores and specifications
        context["when withdrawing cash"] = () =>
        {
            before = () => account = new Account();
            context["account is in credit"] = () =>
            {
                before = () => account.Balance = 500;
                it["the Account dispenses cash"] = () => account.CanWithdraw(60).should_be_true();
            };
            context["account is overdrawn"] = () =>
            {
                before = () => account.Balance = -500;
                it["the Account does not dispense cash"] = () => account.CanWithdraw(60).should_be_false();
            };
        };
    }
    List<int> fibonaccis;
    private Account account;
}