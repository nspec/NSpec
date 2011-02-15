using NSpec;
using NUnit.Framework;
using SampleSpecs;

namespace NSpecSpec
{
    [TestFixture]
    public class when_executing_a_spec_method_directly
    {
        [Test]
        public void should_work()
        {
            var spec = new action_indexer_approach();

            spec.Context = new Context("test");

            spec.a_user();
        }
    }
}