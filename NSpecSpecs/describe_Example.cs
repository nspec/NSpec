using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class describe_Example
    {
        [Test]
        public void should_concatenate_its_contexts_name_into_a_full_name()
        {
            var context = new Context("context name");

            var example = new Example("example name");

            context.AddExample(example);

            example.FullName().should_be("context name. example name.");
        }
    }
}