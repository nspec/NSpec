using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;

class describe_changing_failure_exception : nspec
{
    void given_a_context_that_throws_an_exception()
    {
        it["the exception can be changed to provide out of proc information"] = () =>
            "1".should_be("2");
    }

    public override Exception ExceptionToReturn(Exception originalException)
    {
        return new InvalidOperationException("A more detailed exception message.", originalException);
    }
}

public static class describe_changing_failure_exception_output
{
    public static string Output = @"
describe changing failure exception
  given a context that throws an exception
    the exception can be changed to provide out of proc information - FAILED - A more detailed exception message.

**** FAILURES ****

nspec. describe changing failure exception. given a context that throws an exception. the exception can be changed to provide out of proc information.
A more detailed exception message.
   at describe_changing_failure_exception.<given_a_context_that_throws_an_exception>b__0() in SampleSpecs\WebSite\describe_changing_failure_exception.cs:line 12

1 Examples, 1 Failed, 0 Pending
";
    public static int ExitCode = 1;
}
