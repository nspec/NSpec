using System;
using System.Linq;
using NSpec;
using NUnit.Framework;
using NSpec.Domain;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_expected_exception : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                before = () => { };

                it["should throw exception"] = expect<InvalidOperationException>(() => { throw new InvalidOperationException(); });

                it["should throw exception with error message Testing"] = expect<InvalidOperationException>( "Testing", () => { throw new InvalidOperationException("Testing"); });

                it["should fail if no exception thrown"] = expect<InvalidOperationException>(() => { });

                it["should fail if wrong exception thrown"] = expect<InvalidOperationException>(() => { throw new ArgumentException(); });

                it["should fail if wrong error message is returned"] = expect<InvalidOperationException>("Testing", () => { throw new InvalidOperationException("Blah"); });
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void should_be_two_failures()
        {
            classContext.Failures().Count().should_be(3);
        }

        [Test]
        public void given_exception_is_thrown_should_not_fail()
        {
            TheExample("should throw exception").Exception.should_be(null);
        }

        [Test]
        public void given_exception_is_thrown_with_expected_message_should_not_fail()
        {
            TheExample("should throw exception with error message Testing").Exception.should_be(null);
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
            TheExample("should fail if wrong exception thrown").Exception.Message.should_be("Exception of type InvalidOperationException was not thrown.");
        }

        [Test]
        public void given_wrong_error_message_should_fail()
        {
            TheExample("should fail if wrong error message is returned").Exception.GetType().should_be(typeof(ExceptionNotThrown));
            TheExample("should fail if wrong error message is returned").Exception.Message.should_be("Expected message: \"Testing\" But was: \"Blah\"");
        }

        private Example TheExample(string name)
        {
            return classContext.Contexts.First().AllExamples().Single(s => s.Spec == name);
        }
    }
}