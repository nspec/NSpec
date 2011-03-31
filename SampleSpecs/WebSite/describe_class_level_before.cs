using NSpec;

class describe_class_level_before : nspec
{
    Account account;

    //nspec will look for a method that is named before_each()
    //the account will be initialized for each test
    void before_each()
    {
        account = new Account();
    }

    void describe_Account()
    {
        context["when withdrawing cash"] = () =>
        {
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
}