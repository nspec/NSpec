using NSpec;

namespace SampleSpecs.Demo
{
    class describe_To : nspec
    {
        void when_creating_ranges()
        {
            it["1.To(2) should be [1,2]"]= () => 1.To(2).should_be(1,2); 
        }
    }
}