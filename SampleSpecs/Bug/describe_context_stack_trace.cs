using System;
using System.IO;
using NSpec;

namespace SampleSpecs.Bug
{
    class describe_context_stack_trace : nspec
    {
        bool isTrue = false;

        void exception_thrown_in_act()
        {
            act = () =>
            {
                MethodThrowsExceptionAndShouldBeInStackTrace();//

                isTrue = true;
            };

            it["is true"] = () => isTrue.should_be_true();
        }

        void MethodThrowsExceptionAndShouldBeInStackTrace()
        {
            throw new InvalidOperationException("Exception in act.");//
        }
    }

    public static class describe_context_stack_trace_output
    {
        public static string Output = string.Format(@"
describe context stack trace
  exception thrown in act
    is true - FAILED - Context Failure: Exception in act., Example Failure: Expected: True, But was: False

**** FAILURES ****

nspec. describe context stack trace. exception thrown in act. is true.
Context Failure: Exception in act., Example Failure: Expected: True, But was: False
   at SampleSpecs.Bug.describe_context_stack_trace.MethodThrowsExceptionAndShouldBeInStackTrace() in {0}\SampleSpecs\Bug\describe_context_stack_trace.cs:line 25
   at SampleSpecs.Bug.describe_context_stack_trace.<exception_thrown_in_act>b__0() in {0}\SampleSpecs\Bug\describe_context_stack_trace.cs:line 15

1 Examples, 1 Failed, 0 Pending
", Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.Replace(@"NSpecSpecs\bin\Debug", string.Empty)).Replace("C:\\", "c:\\"));
    }
}
