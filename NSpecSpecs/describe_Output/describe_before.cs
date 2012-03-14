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
            Run("describe_before").Is(@"
describe before
  they run before each example
    number should be 2
    number should be 1

2 Examples, 0 Failed, 0 Pending
");
        }
    }
}
