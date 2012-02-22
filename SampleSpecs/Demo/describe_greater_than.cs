using System;
using NSpec;

namespace SampleSpecs.Demo
{
    class describe_greater_than :nspec
    {
        void it_evaluates_passing_examples_correctly()
        {
            3.should_be_greater_than(2);
        }

        void it_evaluates_failing_examples_correctly()
        {
            expect<Exception>(() => 1.should_be_greater_than(1));
        }
    }
}