using NUnit.Framework;
using NSpec.Domain;
using NSpec.Assertions;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_parsing_expressions
    {
        [Test]
        public void should_clear_quotes()
        {
            new Example(() => "hello".should_be("hello")).Spec.should_be("hello should be hello");
        }
    }
}
