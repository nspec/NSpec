using System;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_expected_exception : when_expecting_exception
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                before = () => { };

                it["throws expected exception"] = expect<InvalidOperationException>(() => { throw new InvalidOperationException(); });

                it["throws expected exception with expected error message"] = expect<InvalidOperationException>("Testing", () => { throw new InvalidOperationException("Testing"); });

                it["fails if expected exception does not throw"] = expect<InvalidOperationException>(() => { });

                it["fails if wrong exception thrown"] = expect<InvalidOperationException>(() => { throw new ArgumentException(); });

                it["fails if wrong error message is returned"] = expect<InvalidOperationException>("Testing", () => { throw new InvalidOperationException("Blah"); });
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_expected_exception : when_expecting_exception
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                before = () => { };

                itAsync["throws expected exception"] = expectAsync<InvalidOperationException>(async () => 
                    await Task.Run(() => 
                    { 
                        throw new InvalidOperationException(); 
                    }));

                itAsync["throws expected exception with expected error message"] = expectAsync<InvalidOperationException>("Testing", async () => 
                    await Task.Run(() => { throw new InvalidOperationException("Testing"); }));

                itAsync["fails if expected exception does not throw"] = expectAsync<InvalidOperationException>(async () => 
                    await Task.Run(() => { }));

                itAsync["fails if wrong exception thrown"] = expectAsync<InvalidOperationException>(async () => 
                    await Task.Run(() => { throw new ArgumentException(); }));

                itAsync["fails if wrong error message is returned"] = expectAsync<InvalidOperationException>("Testing", async () => 
                    await Task.Run(() => { throw new InvalidOperationException("Blah"); }));
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }
    }

    public abstract class when_expecting_exception : when_running_specs
    {
        [Test]
        public void should_be_three_failures()
        {
            classContext.Failures().Count().should_be(3);
        }

        [Test]
        public void throws_expected_exception()
        {
            TheExample("throws expected exception").should_have_passed();
        }

        [Test]
        public void throws_expected_exception_with_error_message_Testing()
        {
            TheExample("throws expected exception with expected error message").should_have_passed();
        }

        [Test]
        public void fails_if_expected_exception_not_thrown()
        {
            TheExample("fails if expected exception does not throw").Exception.GetType().should_be(typeof(ExceptionNotThrown));
        }

        [Test]
        public void fails_if_wrong_exception_thrown()
        {
            var exception = TheExample("fails if wrong exception thrown").Exception;

            exception.GetType().should_be(typeof(ExceptionNotThrown));
            exception.Message.should_be("Exception of type InvalidOperationException was not thrown.");
        }

        [Test]
        public void fails_if_wrong_error_message_is_returned()
        {
            var exception = TheExample("fails if wrong error message is returned").Exception;

            exception.GetType().should_be(typeof(ExceptionNotThrown));
            exception.Message.should_be("Expected message: \"Testing\" But was: \"Blah\"");
        }
    }
}