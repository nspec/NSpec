using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpec.Tests.describe_RunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_before : when_describing_async_hooks
    {
        class SpecClass : BaseSpecClass
        {
            void given_async_before_is_set()
            {
                beforeAsync = SetStateAsync;

                it["Should have final value"] = ShouldHaveFinalState;
            }

            void given_async_before_fails()
            {
                beforeAsync = FailAsync;

                it["Should fail"] = () => Assert.That(true, Is.True);
            }

            void given_both_sync_and_async_before_are_set()
            {
                before = SetAnotherState;

                beforeAsync = SetStateAsync;

                it["Should not know what to expect"] = () => Assert.That(true, Is.True);
            }

            void given_before_is_set_to_async_lambda()
            {
                before = async () => { await Task.Delay(0); };

                it["Should fail because before is set to async lambda"] = () => Assert.That(true, Is.True);

                // No chance of error when (async) return value is explicitly typed. The following do not even compile:
                /*
                Func<Task> asyncTaggedDelegate = async () => { await Task.Delay(0); };
                Func<Task> asyncUntaggedDelegate = () => { return Task.Delay(0); };

                // set to async method
                before = SetStateAsync;

                // set to async tagged delegate
                before = asyncTaggedDelegate;

                // set to async untagged delegate
                before = asyncUntaggedDelegate;
                */
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void async_before_waits_for_task_to_complete()
        {
            ExampleRunsWithExpectedState("Should have final value");
        }

        [Test]
        public void async_before_with_exception_fails()
        {
            ExampleRunsWithException("Should fail");
        }

        [Test]
        public void context_with_both_sync_and_async_before_always_fails()
        {
            ExampleRunsWithInnerAsyncMismatchException("Should not know what to expect");
        }

        [Test]
        public void sync_before_set_to_async_lambda_fails()
        {
            ExampleRunsWithInnerAsyncMismatchException("Should fail because before is set to async lambda");
        }
    }
}
