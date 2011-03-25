using System;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_formatting_a_failure
    {
        [SetUp]
        public void setup()
        {
            try
            {
                throw new Exception("BOOM!");
            }
            catch (Exception exception)
            {
                example = new Example("example name") { Exception = exception };
            }

            var context = new Context("context name");

            context.AddExample(example);

            output = new ConsoleFormatter().WriteFailure(example);
        }

        [Test]
        public void should_write_the_examples_full_name()
        {
            output.should_contain(example.FullName());
        }

        [Test]
        public void should_write_the_stack_trace()
        {
            output.should_contain(example.Exception.StackTrace);
        }

        private string output;
        private Example example;
    }
}