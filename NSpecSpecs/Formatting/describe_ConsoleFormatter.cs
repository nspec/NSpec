using System;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class describe_ConsoleFormatter
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

    public static class SpecExtension
    {
        public static void should_be_of_form(this string result, string expected)
        {
            result.should_be(expected.With(Environment.NewLine, "  "));
        }
    }
}