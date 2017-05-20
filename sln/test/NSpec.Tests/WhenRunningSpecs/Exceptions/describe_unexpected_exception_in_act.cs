using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

namespace NSpec.Tests.WhenRunningSpecs.Exceptions
{
    [TestFixture]
    public class describe_unexpected_exception_in_act_and_in_example : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                context["when exception thrown from act and example itself has a failure"] = () =>
                {
                    act = () =>
                    {
                        throw new ActException();
                    };

                    it["reports act failure and example failure"] = () =>
                    {
                        throw new ItException();
                    };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void should_report_both_act_failure_and_example_failure()
        {
            TheExample("reports act failure and example failure")
                .Exception.Message.Should().Be("Context Failure: ActException, Example Failure: ItException");
        }
    }

    [TestFixture]
    public class describe_unexpected_exception_in_act_but_not_example : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                context["when exception thrown from act but not from example"] = () =>
                {
                    act = () =>
                    {
                        throw new ActException();
                    };

                    it["reports act failure only"] = () =>
                    {
                        Assert.That(true, Is.True);
                    };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void should_report_act_failure_only()
        {
            TheExample("reports act failure only")
                .Exception.Message.Should().Be("Context Failure: ActException");
        }
    }
    [TestFixture]
    public class describe_unexpected_exception_in_async_act_and_in_async_example : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                context["when exception thrown from act and example itself has a failure"] = () =>
                {
                    actAsync = async () => await Task.Run(() =>
                    {
                        throw new ActException();
                    });

                    itAsync["reports act failure and example failure"] = async () => await Task.Run(() =>
                    {
                        throw new ItException();
                    });
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void should_report_both_act_failure_and_example_failure()
        {
            TheExample("reports act failure and example failure")
                .Exception.Message.Should().Be("Context Failure: ActException, Example Failure: ItException");
        }
    }

    [TestFixture]
    public class describe_unexpected_exception_in_async_act_but_not_async_example : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                context["when exception thrown from act but not from example"] = () =>
                {
                    actAsync = async () => await Task.Run(() =>
                    {
                        throw new ActException();
                    });

                    itAsync["reports act failure only"] = async () =>
                    {
                        await Task.Delay(0);

                        Assert.That(true, Is.True);
                    };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void should_report_act_failure_only()
        {
            TheExample("reports act failure only")
                .Exception.Message.Should().Be("Context Failure: ActException");
        }
    }
}
