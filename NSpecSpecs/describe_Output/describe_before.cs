using NUnit.Framework;
using NSpec;

namespace NSpecSpecs.describe_Output
{
    [TestFixture]
    public class describe_before : when_run_by_NSpecRunner
    {
        [Test]
        public void outputs_properly()
        {
            Run("describe_before").Is(describe_before_expected.Output);
        }
    }
}
