using FluentAssertions;
using NSpec;
using System;

namespace SampleSpecs.Demo
{
    class SomeSharedSpec : nspec
    {
    }

    class when_inherting_from_some_shared_spec : SomeSharedSpec
    {
        void should_still_run_tests()
        {
            specify = () => "Test".Should().Be("Test", "");
        }
    }
}
