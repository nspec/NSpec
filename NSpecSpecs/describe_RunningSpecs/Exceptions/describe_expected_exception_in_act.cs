using System;
using System.Linq;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using NSpec.Domain;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_expected_exception_in_act : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                it["should fail if no exception thrown"] = expect<InvalidOperationException>();

                context["when exception thrown from act"] = () =>
                {
                    act = () => { throw new InvalidOperationException("Testing"); };

                    it["should throw exception"] = expect<InvalidOperationException>();

                    it["should throw exception with error message Testing"] = expect<InvalidOperationException>("Testing");

                    it["should fail if wrong exception thrown"] = expect<ArgumentException>();

                    it["should fail if wrong error message is returned"] = expect<InvalidOperationException>("Blah");
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Init(typeof(SpecClass)).Run();
        }

        [Test]
        public void should_be_two_failures()
        {
            classContext.Failures().Count().should_be(3);
        }

        [Test]
        public void given_exception_is_thrown_should_not_fail()
        {
            TheExample("should throw exception").should_not_have_failed();
        }

        [Test]
        public void given_exception_is_thrown_with_expected_message_should_not_fail()
        {
            TheExample("should throw exception with error message Testing").should_not_have_failed();
        }

        [Test]
        public void given_exception_not_thrown_should_fail()
        {
            TheExample("should fail if no exception thrown").Exception.GetType().should_be(typeof(ExceptionNotThrown));
        }

        [Test]
        public void given_wrong_exception_should_fail()
        {
            TheExample("should fail if wrong exception thrown").Exception.GetType().should_be(typeof(ExceptionNotThrown));
            TheExample("should fail if wrong exception thrown").Exception.Message.should_be("Exception of type ArgumentException was not thrown.");
        }

        [Test]
        public void given_wrong_error_message_should_fail()
        {
            TheExample("should fail if wrong error message is returned").Exception.GetType().should_be(typeof(ExceptionNotThrown));
            TheExample("should fail if wrong error message is returned").Exception.Message.should_be("Expected message: \"Blah\" But was: \"Testing\"");
        }
    }
}