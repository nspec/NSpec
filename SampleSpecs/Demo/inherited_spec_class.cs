using NSpec;

namespace SampleSpecs.Demo
{
    class SomeSharedSpec : nspec
    {
    }

    class when_inherting_from_some_shared_spec : SomeSharedSpec
    {
        void should_still_run_tests()
        {
            specify = () => "Test".should_be("Test");
        }
    }
}
