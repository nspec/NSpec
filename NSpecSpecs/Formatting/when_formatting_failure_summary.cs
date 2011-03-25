using System;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class given_no_failures : when_formatting_failure_summary
    {
        [Test]
        public void given_no_failures_should_be_empty()
        {
            Given(new Context()).should_be_empty();
        }
    }

    [TestFixture]
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
            output.should_contain(boom.Message + Environment.NewLine + boom.StackTrace + Environment.NewLine.Times(2) );
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
                                          context.AddExample(new Example("example name") { Exception = e });
                                          return context;
                                      }).ToArray());
        }
        protected string Given(params Context[] contexts)
        {
            return new ConsoleFormatter().FailureSummary(contexts);
        }
    }
}