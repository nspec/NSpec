using FluentAssertions;

namespace SampleSpecs
{
    public static class AssertionExtensions
    {
        public static void should_not_be(this object actual, object expected)
        {
            actual.Should().NotBe(expected);
        }
    }
}
