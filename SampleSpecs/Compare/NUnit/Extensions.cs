using NUnit.Framework;

namespace SampleSpecs.Compare.NUnit
{
    public static class Extensions
    {
        public static void ShouldBeTrue(this bool actual)
        {
            Assert.IsTrue( actual);
        }
        public static void ShouldBe(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}