using NSpec.Domain;

namespace NSpec.Assertions
{
    public static class AssertionExtensions
    {
        public static void IsTrue(this bool actual)
        {
            actual.ShouldBeTrue();
        }

        public static void ShouldBeTrue(this bool actual)
        {
            if (!actual)
            {
                throw new AssertionException($"Expected true, but was ${actual}.");
            }
        }

        public static void IsFalse(this bool actual)
        {
            actual.ShouldBeFalse();
        }

        public static void ShouldBeFalse(this bool actual)
        {
            if (actual)
            {
                throw new AssertionException($"Expected false, but was ${actual}.");
            }
        }
    }
}
