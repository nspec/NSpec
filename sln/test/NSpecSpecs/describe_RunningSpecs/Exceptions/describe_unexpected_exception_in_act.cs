using System;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
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
                        throw new KnownException("unexpected failure");
                    };

                    it["reports example level failure and act failure"] = () =>
                    {
                        throw new KnownException("example level failure");
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
        public void should_report_both_method_level_failure_and_act_level_failure()
        {
            TheExample("reports example level failure and act failure")
                .Exception.Message.Should().Be("Context Failure: unexpected failure, Example Failure: example level failure");
        }
    }

    [TestFixture]
    public class describe_unexpected_exception_in_act_but_not_example : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                context["when exception thrown from act and example itself has a failure"] = () =>
                {
                    act = () =>
                    {
                        throw new KnownException("unexpected failure");
                    };

                    it["reports example level failure and act failure"] = () =>
                    {
                        "expected".Should().Be("expected");
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
        public void should_report_both_method_level_failure_and_act_level_failure()
        {
            TheExample("reports example level failure and act failure")
                .Exception.Message.Should().Be("Context Failure: unexpected failure");
        }
    }
    [TestFixture]
    public class describe_unexpected_exception_in_async_act_and_in_example : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                context["when exception thrown from act and example itself has a failure"] = () =>
                {
                    actAsync = async () => await Task.Run(() =>
                    {
                        throw new KnownException("unexpected failure");
                    });

                    itAsync["reports example level failure and act failure"] = async () => await Task.Run(() =>
                    {
                        throw new KnownException("example level failure");
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
        public void should_report_both_method_level_failure_and_act_level_failure()
        {
            TheExample("reports example level failure and act failure")
                .Exception.Message.Should().Be("Context Failure: unexpected failure, Example Failure: example level failure");
        }
    }

    [TestFixture]
    public class describe_unexpected_exception_in_async_act_but_not_example : when_running_specs
    {
        private class SpecClass : nspec
        {
            void method_level_context()
            {
                context["when exception thrown from act and example itself has a failure"] = () =>
                {
                    actAsync = async () => await Task.Run(() =>
                    {
                        throw new KnownException("unexpected failure");
                    });

                    itAsync["reports example level failure and act failure"] = async () =>
                    {
                        await Task.Delay(0);

                        "expected".Should().Be("expected");
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
        public void should_report_both_method_level_failure_and_act_level_failure()
        {
            TheExample("reports example level failure and act failure")
                .Exception.Message.Should().Be("Context Failure: unexpected failure");
        }
    }
}
