using NSpec.Domain.Formatters;
using NUnit.Framework;

namespace NSpecSpecs
{
    [TestFixture]
    public class describe_ConsoleFormatter
    {
        private ConsoleFormatter formatter;

        [SetUp]
        public void Setup()
        {
            this.formatter = new ConsoleFormatter();
        }
        
        [Test]
        public void given_a_context_name_ending_with_specs_should_strip_it_from_end()
        {
            string result = formatter.StripOffSpecsTextFromContextName("HandySpecs");

            Assert.AreEqual("Handy", result);
        }
    }
}
