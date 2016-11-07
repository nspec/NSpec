using FluentAssertions;
using NSpec;

namespace SampleSpecs.Demo
{
    class describe_shoulds : nspec
    {
        void given_a_non_empty_list()
        {
            it["should not be empty"] = () => new [] { 1 }.Should().NotBeEmpty();
        }
    }
}