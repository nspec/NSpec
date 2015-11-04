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
    public class describe_expected_exception_in_act : when_expecting_exception_in_act
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                it["fails if no exception thrown"] = expect<InvalidOperationException>();

                context["when exception thrown from act"] = () =>
                {
                    act = () => { throw new InvalidOperationException("Testing"); };

                    it["threw the expected exception in act"] = expect<InvalidOperationException>();

                    it["threw the exception in act with expected error message"] = expect<InvalidOperationException>("Testing");

                    it["fails if wrong exception thrown"] = expect<ArgumentException>();

                    it["fails if wrong error message is returned"] = expect<InvalidOperationException>("Blah");
                };
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
    public class describe_expected_exception_in_async_act_before_awaiting_a_task : when_expecting_exception_in_act
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                it["fails if no exception thrown"] = expect<InvalidOperationException>();

                context["when exception thrown from act"] = () =>
                {
                    actAsync = () => Task.Run(() => { throw new InvalidOperationException("Testing"); });

                    it["threw the expected exception in act"] = expect<InvalidOperationException>();

                    it["threw the exception in act with expected error message"] = expect<InvalidOperationException>("Testing");

                    it["fails if wrong exception thrown"] = expect<ArgumentException>();

                    it["fails if wrong error message is returned"] = expect<InvalidOperationException>("Blah");
                };
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
    public class describe_expected_exception_in_async_act_after_awaiting_a_task : when_expecting_exception_in_act
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                it["fails if no exception thrown"] = expect<InvalidOperationException>();

                context["when exception thrown from act after awaiting another task"] = () =>
                {
                    actAsync = async () =>
                    {
                        await Task.Run(() => { } );

                        throw new InvalidOperationException("Testing");
                    };

                    it["threw the expected exception in act"] = expect<InvalidOperationException>();

                    it["threw the exception in act with expected error message"] = expect<InvalidOperationException>("Testing");

                    it["fails if wrong exception thrown"] = expect<ArgumentException>();

                    it["fails if wrong error message is returned"] = expect<InvalidOperationException>("Blah");
                };
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
    public class describe_expected_exception_in_async_act_within_list_of_tasks : when_expecting_exception_in_act
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                it["fails if no exception thrown"] = expect<InvalidOperationException>();

                context["when exception thrown from act within a list of tasks"] = () =>
                {
                    actAsync = () =>
                    {
                        var tasks = Enumerable.Range(0, 10)
                            .Select(e => Task.Run(() => {
                                if (e == 4)
                                {
                                    throw new InvalidOperationException("Testing");
                                }
                            }));

                        return Task.WhenAll(tasks);
                    };

                    it["threw the expected exception in act"] = expect<InvalidOperationException>();

                    it["threw the exception in act with expected error message"] = expect<InvalidOperationException>("Testing");

                    it["fails if wrong exception thrown"] = expect<ArgumentException>();

                    it["fails if wrong error message is returned"] = expect<InvalidOperationException>("Blah");
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }
    }

    public abstract class when_expecting_exception_in_act : when_running_specs
    {
        [Test]
        public void should_be_three_failures()
        {
            classContext.Failures().Count().should_be(3);
        }

        [Test]
        public void threw_expected_exception_in_act()
        {
            TheExample("threw the expected exception in act").should_have_passed();
        }

        [Test]
        public void threw_the_exception_in_act_with_the_proper_error_message()
        {
            TheExample("threw the exception in act with expected error message").should_have_passed();
        }

        [Test]
        public void fails_if_no_exception_thrown()
        {
            TheExample("fails if no exception thrown").Exception.GetType().should_be(typeof(ExceptionNotThrown));
        }

        [Test]
        public void fails_if_wrong_exception_thrown()
        {
            var exception = TheExample("fails if wrong exception thrown").Exception;

            exception.GetType().should_be(typeof(ExceptionNotThrown));
            exception.Message.should_be("Exception of type ArgumentException was not thrown.");
        }

        [Test]
        public void fails_if_wrong_error_message_is_returned()
        {
            var exception = TheExample("fails if wrong error message is returned").Exception;

            exception.GetType().should_be(typeof(ExceptionNotThrown));
            exception.Message.should_be("Expected message: \"Blah\" But was: \"Testing\"");
        }
    }
}