using System;
using NSpec.Assertions;
using NSpec.Domain;
using NSpec.Extensions;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_formatting_console_output
    {
        [SetUp]
        public void setup()
        {
            formatter = new ConsoleFormatter();
        }

        [Test]
        public void given_a_context()
        {
            formatter.Write(new Context("hello_world")).should_be("hello world");
        }

        [Test]
        public void given_nested_contexts()
        {
            var parent = new Context("parent context");

            var child = new Context("child context");

            var grandChild = new Context("grandchild context");

            parent.AddContext(child);

            child.AddContext(grandChild);

            formatter.Write(parent).should_be_of_form("parent context{0}{1}child context{0}{1}{1}grandchild context");
        }

        private ConsoleFormatter formatter;
    }

    public class when_formatting_examples
    {
        [SetUp]
        public void setup()
        {
            output = new ConsoleFormatter().Write(new Example("An Example"));
        }
        [Test]
        public void should_prepend_with_new_line()
        {
            output.should_start_with(Environment.NewLine);
        }

        [Test]
        public void should_also_prepend_indent()
        {
           output.should_end_with("  An Example");
        }

        private string output;
    }

    public class when_formatting_failed_examples
    {
        [SetUp]
        public void setup()
        {
            exceptionMessage = "Exception Message";

            output = new ConsoleFormatter().Write(new Example("An Failed Example"){Exception = new Exception(exceptionMessage)});
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

    public class when_formatting_exceptions
    {
        [Test]
        public void should_collapse_spaces()
        {
            Given("double space  should collapse").should_be("double space should collapse");
        }

        [Test]
        public void should_trim_spaces_and_new_lines()
        {
            Given(" {0}trimmed   {0}".With(Environment.NewLine)).should_be("trimmed");
        }

        private string Given(string message)
        {
            return new ConsoleFormatter().WriteException(new Exception(message));
        }
    }

    public static class SpecExtension
    {
        public static void should_be_of_form(this string result, string expected)
        {
            result.should_be(expected.With(Environment.NewLine, "  "));
        }
    }
}