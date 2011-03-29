using NSpec;

namespace SampleSpecs.WebSite
{
    class describe_specifications : nspec
    {
        void when_creating_specifications()
        {
            it["true should be false"] = () => true.should_be_false();
            it["enumerable should be empty"] = () => new int[]{}.should_be_empty();
            it["enumerable should contain 1"] = () => new[]{1}.should_contain(1);
            it["enumerable should not contain 1"] = () => new[]{1}.should_contain(2);
            it["1 should be 2"] = () => 1.should_be(2);
            it["1 should be 1"] = () => 1.should_be(1);
            it["1 should not be 1"] = () => 1.should_not_be(1);
            it["1 should not be 2"] = () => 1.should_not_be(2);
            it["\"\" should not be null"] = () => "".should_not_be_null();
            it["spec should not be null"] = () => spec.should_not_be_null();
        }
        object spec;
    }
}