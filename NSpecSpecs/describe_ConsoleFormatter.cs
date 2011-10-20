using System;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NUnit.Framework;
using NSpec.Domain.Extensions;

namespace NSpecNUnit.describe_ConsoleFormatter
{
    [TestFixture]
    [Category("ConsoleFormatter")]
    public class when_formatting_contexts
    {
        [SetUp]
        public void setup()
        {
            formatter = new ConsoleFormatter();
        }

        [Test]
        public void should_prepend_context_name_with_newline()
        {
            formatter.Write(new Context("hello world")).should_be_of_form("{0}hello world");
        }

        [Test]
        public void should_prepend_successive_subcontexts_with_additional_indents()
        {
            var parent = new Context("parent context");

            var child = new Context("child context");

            var grandChild = new Context("grandchild context");

            parent.AddContext(child);

            child.AddContext(grandChild);

            formatter.Write(parent).should_be_of_form("{0}parent context{0}{1}child context{0}{1}{1}grandchild context");
        }

        private ConsoleFormatter formatter;
    }

    [TestFixture]
    [Category("ConsoleFormatter")]
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
                example = new Example("example name") { ExampleLevelException = exception };
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
            output.should_contain(example.ExampleLevelException.StackTrace);
        }

        [Test]
        public void should_exclude_stack_trace_elements_that_have_an_empty_line()
        {

        }

        private string output;
        private Example example;
    }

    [TestFixture]
    [Category("ConsoleFormatter")]
    public class when_formatting_examples
    {
        [Test]
        public void should_prepend_name_with_new_line_and_indent()
        {
            new ConsoleFormatter().Write(new Example()).should_start_with(Environment.NewLine + "  ");
        }
    }

    [TestFixture]
    [Category("ConsoleFormatter")]
    public class when_formatting_failed_examples
    {
        [SetUp]
        public void setup()
        {
            exceptionMessage = "Exception Message";

            output = new ConsoleFormatter().Write(new Example("An Failed Example") { ExampleLevelException = new Exception(exceptionMessage) });
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

    [TestFixture]
    [Category("ConsoleFormatter")]
    public class when_formatting_exceptions
    {
        [Test]
        public void should_collapse_spaces()
        {
            CleanExceptionMessage("double space  should collapse").should_be("double space should collapse");
        }

        [Test]
        public void should_trim_spaces_and_new_lines()
        {
            CleanExceptionMessage(" {0}trimmed   {0}".With(Environment.NewLine)).should_be("trimmed");
        }

        private string CleanExceptionMessage(string message)
        {
            return new Exception(message).CleanMessage();
        }
    }

    [TestFixture]
    [Category("ConsoleFormatter")]
    public class given_no_failures : when_formatting_failure_summary
    {
        [Test]
        public void given_no_failures_should_be_empty()
        {
            Given(new Context()).should_be_empty();
        }
    }

    [TestFixture]
    [Category("ConsoleFormatter")]
    public class given_failures : when_formatting_failure_summary
    {
        [SetUp]
        public void setup()
        {
            try
            {
                throw new Exception("BOOM!");
            }
            catch (Exception realBoom)
            {
                boom = realBoom;
            }

            smash = new Exception("SMASH!");

            output = Given(boom, smash);
        }

        [Test]
        public void should_start_with_newline()
        {
            output.should_start_with(Environment.NewLine);
        }

        [Test]
        public void should_contain_empasized_FAILURES_section_header()
        {
            output.should_contain("* FAILURES *");
        }

        [Test]
        public void should_write_each_exception_message()
        {
            output.should_contain(boom.Message);
            output.should_contain(smash.Message);
        }

        [Test]
        public void should_separate_failures_with_two_newlines()
        {
            output.should_contain(boom.Message + Environment.NewLine + boom.StackTrace + Environment.NewLine.Times(2));
        }

        string output;
        Exception boom, smash;
    }

    public class when_formatting_failure_summary
    {
        protected string Given(params Exception[] exceptions)
        {
            return Given(
                exceptions.Select(e =>
                {
                    var context = new Context("context name");
                    context.AddExample(new Example("example name") { ExampleLevelException = e });
                    return context;
                }).ToArray());
        }
        protected string Given(params Context[] contexts)
        {
            return new ConsoleFormatter().FailureSummary(new ContextCollection(contexts));
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