using System;
using NSpec;
using FluentAssertions;

namespace SampleSpecs.Bug
{
    public class describe_context_stack_trace : nspec
    {
        bool isTrue = false;

        void exception_thrown_in_act()
        {
            act = () =>
            {
                MethodThrowsExceptionAndShouldBeInStackTrace();

                isTrue = true;
            };

            it["is true"] = () => isTrue.Should().BeTrue();
        }

        void MethodThrowsExceptionAndShouldBeInStackTrace()
        {
            throw new InvalidOperationException("Exception in act.");
        }
    }

    public static class describe_context_stack_trace_output
    {
        public static string Output = @"
describe context stack trace
  exception thrown in act
    is true (__ms) - FAILED - Expected: True, But was: False

**** FAILURES ****

nspec. describe context stack trace. exception thrown in act. is true.
Context Failure: Exception in act., Example Failure: Expected: True, But was: False

1 Examples, 1 Failed, 0 Pending
";
        public static int ExitCode = 1;
    }
}
