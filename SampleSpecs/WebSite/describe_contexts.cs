using NSpec;
using NSpec.Assertions.nUnit;

class describe_contexts : nspec
{
    //context methods require an underscore. For more info see DefaultConventions.cs.
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
    private Account account;
}

public static class describe_contexts_output
{
    public static string Output = @"
describe contexts
  describe Account
    when withdrawing cash
      account is in credit
        the Account dispenses cash
      account is overdrawn
        the Account does not dispense cash

2 Examples, 0 Failed, 0 Pending
";
    public static int ExitCode = 0;
}