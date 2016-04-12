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
    public class describe_async_after_all : when_describing_async_hooks
    {
        class SpecClass : BaseSpecClass
        {
            void given_async_after_is_set()
            {
                it["Should have initial value"] = ShouldHaveInitialState;

                afterAllAsync = SetStateAsync;
            }

            void given_async_after_all_fails()
            {
                it["Should fail"] = PassAlways;

                afterAllAsync = FailAsync;
            }

            void given_both_sync_and_async_after_all_are_set()
            {
                it["Should not know what to do"] = PassAlways;

                afterAll = SetAnotherState;

                afterAllAsync = SetStateAsync;
            }

            void given_after_all_is_set_to_async_lambda()
            {
                afterAll = async () => { await Task.Delay(0); };

                it["Should fail because afterAll is set to async lambda"] = PassAlways;

                // No chance of error when (async) return value is explicitly typed. The following do not even compile:
                /*
                Func<Task> asyncTaggedDelegate = async () => { await Task.Delay(0); };
                Func<Task> asyncUntaggedDelegate = () => { return Task.Delay(0); };

                // set to async method
                afterAll = SetStateAsync;

                // set to async tagged delegate
                afterAll = asyncTaggedDelegate;

                // set to async untagged delegate
                afterAll = asyncUntaggedDelegate;
                */
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void async_after_all_waits_for_task_to_complete()
        {
            ExampleRunsWithExpectedState("Should have initial value");
        }

        [Test]
        public void async_after_all_with_exception_fails()
        {
            ExampleRunsWithException("Should fail");
        }

        [Test]
        public void context_with_both_sync_and_async_after_all_always_fails()
        {
            ExampleRunsWithException("Should not know what to do");
        }

        [Test]
        public void sync_after_all_set_to_async_lambda_fails()
        {
            ExampleRunsWithException("Should fail because afterAll is set to async lambda");
        }
    }
}
