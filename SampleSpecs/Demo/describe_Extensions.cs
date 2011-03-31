using NSpec;

namespace SampleSpecs.Demo
{
    class describe_Extensions : nspec
    {
        void when_creating_ranges()
        {
            it["1.To(2) should be [1,2]"]= () => 1.To(2).should_be(1,2); 
        }

        void describe_Flatten()
        {
            it["[\"fifty\",\"two\"] should be fiftytwo"] = () => new[] { "fifty", "two" }.Flatten(",").should_be("fifty,two");
        }
    }
}