using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;

namespace NSpecSpecs.describe_RunningSpecs
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
                        throw new InvalidOperationException("unexpected failure"); 
                    };

                    it["reports example level failure and act failure"] = () => 
                    {
                        throw new InvalidOperationException("example level failure");
                    };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Init(typeof(SpecClass));

            Run();
        }

        [Test]
        public void should_report_both_method_level_failure_and_act_level_failure()
        {
            TheExample("reports example level failure and act failure")
                .Exception.Message.should_be("Context Failure: unexpected failure, Example Failure: example level failure");
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
                        throw new InvalidOperationException("unexpected failure");
                    };

                    it["reports example level failure and act failure"] = () =>
                    {
                        "expected".should_be("expected");
                    };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Init(typeof(SpecClass)).Run();
        }

        [Test]
        public void should_report_both_method_level_failure_and_act_level_failure()
        {
            TheExample("reports example level failure and act failure")
                .Exception.Message.should_be("Context Failure: unexpected failure");
        }
    }
}
