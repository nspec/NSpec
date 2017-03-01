using NSpec;
using NSpec.Assertions;
using SampleSpecsApi.SampleSystem;





namespace SampleSpecsApi
{
    // Do not move the following spec classes around, to avoid rewriting line numbers. This should be stuck at line nr. 11
    class ParentSpec : nspec
    {
        protected SystemUnderTest systemUnderTest;

        void before_each()
        {
            systemUnderTest = new SystemUnderTest();
        }

        [Tag("Tag-1A Tag-1B")]
        void method_context_1()
        {
            it["parent example 1A"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 24

            it["parent example 1B"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 26
        }

        void method_context_2()
        {
            it["parent example 2A"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 31
        }
    }

    [Tag("Tag-Child")]
    class ChildSpec : ParentSpec
    {
        [Tag("Tag-Child-example-skipped")]
        void method_context_3()
        {
            it["child example 3A skipped"] = todo; // # 41
        }

        [Tag("Tag_with_underscores")]
        void method_context_4()
        {
            it["child example 4A"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 47
        }

        void method_context_5()
        {
            context["sub context 5-1"] = () =>
            {
                it["child example 5-1A failing"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeFalse(); // # 54

                it["child example 5-1B"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 56
            };

            it["child example 5A"] = () => systemUnderTest.IsAlwaysTrue().ShouldBeTrue(); // # 59
        }

        void it_child_method_example_A()
        { // # 63
            systemUnderTest.IsAlwaysTrue().ShouldBeTrue();
        }
    }

    // Do not move the preceding spec classes around, to avoid rewriting line numbers

    public class PublicPlaceholderClass
    {
    }
}
