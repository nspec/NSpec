using NUnit.Framework;

namespace SampleSpecs.Compare.NUnit
{
    public static class Extensions
    {
        public static void ShouldBe(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}