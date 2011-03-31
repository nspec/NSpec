using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;

namespace SampleSpecs.WebSite
{
    class describe_expected_exception : nspec
    {
        void when_withdrawing_a_negative_amount()
        {
            account = new Account();

            it["should throw exception"] = 
                expect<InvalidOperationException>(() => account.Withdraw(-200));
        }
        Account account;
    }
}
