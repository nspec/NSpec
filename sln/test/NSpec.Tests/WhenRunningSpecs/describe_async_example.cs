using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpec.Tests.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_example : when_describing_async_hooks
    {
        class SpecClass : BaseSpecClass
        {
            void given_async_example_is_set()
            {
                itAsync["Should wait for its task to complete"] = async () =>
                {
                    await SetStateAsync();

                    ShouldHaveFinalState();
                };
            }

            void given_async_example_fails()
            {
                itAsync["Should fail asynchronously"] = FailAsync;
            }

            void given_example_is_set_to_async_lambda()
            {
                it["Should fail because it is set to async lambda"] = async () => { await Task.Delay(0); };

                // No chance of error when (async) return value is explicitly typed. The following do not even compile:
                /*
                Func<Task> asyncTaggedDelegate = async () => { await Task.Delay(0); };
                Func<Task> asyncUntaggedDelegate = () => { return Task.Delay(0); };

                it["Should fail because it is set to async method"] = SetStateAsync;

                it["Should fail because it is set to async tagged delegate"] = asyncTaggedDelegate;

                it["Should fail because it is set to async untagged delegate"] = asyncUntaggedDelegate;
                */
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void async_example_waits_for_task_to_complete()
        {
            ExampleRunsWithExpectedState("Should wait for its task to complete");
        }

        [Test]
        public void async_example_with_exception_fails()
        {
            ExampleRunsWithException("Should fail asynchronously");
        }

        [Test]
        public void sync_example_set_to_async_lambda_fails()
        {
            ExampleRunsWithAsyncMismatchException("Should fail because it is set to async lambda");
        }
    }
}
