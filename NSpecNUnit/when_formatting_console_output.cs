using System;
using System.Collections.Generic;
using System.Linq;
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
            Given(new Context("hello world")).should_be("hello world");
        }

        [Test]
        public void given_a_context_with_a_passing_example()
        {
            Given(new Context("passing example context"){Examples = new[]{new Example("passing example")}.ToList()})
                .should_be("passing example context{0}{1}passing example".With(Environment.NewLine,"\t"));
        }

        [Test]
        public void given_nested_contexts()
        {
            Given(new Context("parent context")
            {
                Contexts=new[]{new Context("child context")
                {
                    Contexts = new []{new Context("grandchild context"){Examples = new[]{new Example("gchild example")}.ToList()}}.ToList()
                }}.ToList()
            })
            .should_be("parent context{0}{1}child context{0}{1}{1}grandchild context{0}{1}{1}{1}gchild example".With(Environment.NewLine,"\t"));
        }

        [Test]
        public void given_a_failing_example()
        {
            Given(new Context("failing context") { Examples = new[] { new Example("failed example") { Exception = new Exception("I blew up") } }.ToList() })
                .should_be("failing context{0}{1}failed example - FAILED - I blew up".With(Environment.NewLine,"\t"));
        }

        private string Given(Context context)
        {
            var write = formatter.Write(context);

            Console.WriteLine(write);

            return write;
        }

        private ConsoleFormatter formatter;
    }
}