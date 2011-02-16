using NSpec.Domain;
using NUnit.Framework;

namespace NSpecSpec
{
    [TestFixture]
    public class when_executing_a_spec_method_directly
    {
        [Test]
        public void should_work()
        {
            var spec = new when_executing_a_context();

            spec.Context = new Context("test");

            spec.a_context();
        }
    }
}