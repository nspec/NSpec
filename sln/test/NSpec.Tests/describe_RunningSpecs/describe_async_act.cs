using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpec.Tests.describe_RunningSpecs
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

                it["Should fail because of exception"] = () => Assert.That(true, Is.True);
            }

            void given_both_sync_and_async_act_are_set()
            {
                act = SetAnotherState;

                actAsync = SetStateAsync;

                it["Should not know what to expect"] = () => Assert.That(true, Is.True);
            }

            void given_act_is_set_to_async_lambda()
            {
                act = async () => { await Task.Delay(0); };

                it["Should fail because act is set to async lambda"] = () => Assert.That(true, Is.True);

                // No chance of error when (async) return value is explicitly typed. The following do not even compile:
                /*
                Func<Task> asyncTaggedDelegate = async () => { await Task.Delay(0); };
                Func<Task> asyncUntaggedDelegate = () => { return Task.Delay(0); };

                // set to async method
                act = SetStateAsync;

                // set to async tagged delegate
                act = asyncTaggedDelegate;

                // set to async untagged delegate
                act = asyncUntaggedDelegate;
                */
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
            ExampleRunsWithInnerAsyncMismatchException("Should not know what to expect");
        }

        [Test]
        public void sync_act_set_to_async_lambda_fails()
        {
            ExampleRunsWithInnerAsyncMismatchException("Should fail because act is set to async lambda");
        }
    }
}
