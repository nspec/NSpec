using System;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_formatting_examples
    {
        [Test]
        public void should_prepend_name_with_new_line_and_indent()
        {
            new ConsoleFormatter().Write(new Example()).should_start_with(Environment.NewLine + "  ");
        }
    }

    [TestFixture]
    public class when_formatting_failed_examples
    {
        [SetUp]
        public void setup()
        {
            exceptionMessage = "Exception Message";

            output = new ConsoleFormatter().Write(new Example("An Failed Example") { Exception = new Exception(exceptionMessage) });
        }

        [Test]
        public void should_indicate_failed()
        {
            output.should_contain(" - FAILED - ");
        }

        [Test]
        public void should_show_the_exception_message()
        {
            output.should_end_with(exceptionMessage);
        }

        private string output;
        private string exceptionMessage;
    }
}