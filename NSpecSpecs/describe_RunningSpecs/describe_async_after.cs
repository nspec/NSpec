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
    public class describe_async_after : when_describing_async_hooks
    {
        class SpecClass : BaseSpecClass
        {
            void given_async_after_is_set()
            {
                it["Should have initial value"] = ShouldHaveInitialState;

                afterAsync = SetStateAsync;
            }

            void given_async_after_fails()
            {
                it["Should fail"] = PassAlways;

                afterAsync = FailAsync;
            }

            void given_both_sync_and_async_after_are_set()
            {
                it["Should not know what to do"] = PassAlways;

                after = SetAnotherState;

                afterAsync = SetStateAsync;
            }

            void given_after_is_set_to_async_lambda()
            {
                after = async () => { await Task.Delay(0); };

                it["Should fail because after is set to async lambda"] = PassAlways;

                // No chance of error when (async) return value is explicitly typed. The following do not even compile:
                /*
                Func<Task> asyncTaggedDelegate = async () => { await Task.Delay(0); };
                Func<Task> asyncUntaggedDelegate = () => { return Task.Delay(0); };

                // set to async method
                after = SetStateAsync;

                // set to async tagged delegate
                after = asyncTaggedDelegate;

                // set to async untagged delegate
                after = asyncUntaggedDelegate;
                */
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void async_after_waits_for_task_to_complete()
        {
            ExampleRunsWithExpectedState("Should have initial value");
        }

        [Test]
        public void async_after_with_exception_fails()
        {
            ExampleRunsWithException("Should fail");
        }

        [Test]
        public void context_with_both_sync_and_async_after_always_fails()
        {
            ExampleRunsWithException("Should not know what to do");
        }

        [Test]
        public void sync_after_set_to_async_lambda_fails()
        {
            ExampleRunsWithException("Should fail because after is set to async lambda");
        }
    }
}
