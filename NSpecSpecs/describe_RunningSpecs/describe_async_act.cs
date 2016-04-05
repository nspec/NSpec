using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_act : when_describing_async_hooks
    {
        class SpecClass : BaseSpecClass
        {
            void given_async_act_is_set()
            {
                actAsync = SetStateAsync;

                it["Should have final value"] = ShouldHaveFinalState;
            }

            void given_async_act_fails()
            {
                actAsync = FailAsync;

                it["Should fail because of exception"] = PassAlways;
            }

            void given_both_sync_and_async_act_are_set()
            {
                act = SetAnotherState;

                actAsync = SetStateAsync;

                it["Should not know what to expect"] = PassAlways;
            }

            void given_sync_act_is_set_to_async_lambda()
            {
                // No chance of error when async function return value is typed explicitly. The following would not even compile:
                // act = SetStateAsync;
                // Error chance arise with async lambda, whose return value is not explicit.

                act = async () => { await Task.Delay(0); };

                it["Should fail because async lambda set to sync"] = PassAlways;
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void async_act_waits_for_task_to_complete()
        {
            ExampleRunsWithExpectedState("Should have final value");
        }

        [Test]
        public void async_act_with_exception_fails()
        {
            ExampleRunsWithException("Should fail because of exception");
        }

        [Test]
        public void context_with_both_sync_and_async_act_always_fails()
        {
            ExampleRunsWithException("Should not know what to expect");
        }

        [Test]
        public void sync_act_set_to_async_lambda_fails()
        {
            ExampleRunsWithException("Should fail because async lambda set to sync");
        }
    }
}
