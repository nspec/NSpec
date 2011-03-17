using System;
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
            formatter.Write(new Context("hello world")).should_be_of_form("hello world");
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

        [Test]
        public void given_a_passing_example()
        {
            formatter.Write(new Example("passing example")).should_be_of_form("{0}{1}passing example");
        }

        [Test]
        public void given_a_failing_example()
        {
            formatter.Write( new Example("failed example") { Exception = new Exception("I blew up") })
                .should_be_of_form("{0}{1}failed example - FAILED - I blew up");
        }

        private ConsoleFormatter formatter;
    }

    public static class SpecExtension
    {
        public static void should_be_of_form(this string result, string expected)
        {
            result.should_be(expected.With(Environment.NewLine, "  "));
        }
    }
}