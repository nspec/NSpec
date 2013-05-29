using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;

class describe_changing_stacktrace_message : nspec
{
    void given_a_context_that_throws_an_exception()
    {
        it["the stack trace can be altered to provide more information"] = () =>
        {
            throw new InvalidOperationException("An exception was thrown");
        };
    }

    public override string StackTraceToPrint(string flattenedStackTrace)
    {
        return flattenedStackTrace + "More Information to help diagnose issue\r\n";
    }
}

public static class describe_changing_stacktrace_message_output
{
    public static string Output = @"
describe changing stacktrace message
  given a context that throws an exception
    the stack trace can be altered to provide more information - FAILED - An exception was thrown

**** FAILURES ****

nspec. describe changing stacktrace message. given a context that throws an exception. the stack trace can be altered to provide more information.
An exception was thrown
   at describe_changing_stacktrace_message.<given_a_context_that_throws_an_exception>b__0() in SampleSpecs\WebSite\describe_changing_stacktrace_message.cs:line 13
More Information to help diagnose issue

1 Examples, 1 Failed, 0 Pending
";
    public static int ExitCode = 1;
}
