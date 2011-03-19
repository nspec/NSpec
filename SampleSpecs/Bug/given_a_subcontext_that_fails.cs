using NSpec.Extensions;
using NSpec;

namespace SampleSpecs.Bug
{
    public class given_a_subcontext_that_fails : spec
    {
        public void when_totaling_failures()
        {
            //could not find a way to exercise this requirement using nspec
            //that didn't require using the broken behavior
            //which led to an impossibility of getting the spec to fail with the broken code
            //and pass with the correct code.... NUnit???
            specify("should count this failure", () => 1.should_be(2));
        }
    }
}