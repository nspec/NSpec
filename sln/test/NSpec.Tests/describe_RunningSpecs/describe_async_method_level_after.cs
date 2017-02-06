using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpec.Tests.describe_RunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_method_level_after : when_describing_async_hooks
    {
        class SpecClass : BaseSpecClass
        {
            void it_should_have_initial_value()
            {
                ShouldHaveInitialState();
            }

            async Task after_each()
            {
                await SetStateAsync();
            }
        }

        [Test]
        public void async_method_level_after_waits_for_task_to_complete()
        {
            Run(typeof(SpecClass));

            ExampleRunsWithExpectedState("it should have initial value");
        }

        class WrongSpecClass : BaseSpecClass
        {
            void it_should_not_know_what_to_do()
            {
                Assert.That(true, Is.True);
            }

            void after_each()
            {
                SetAnotherState();
            }

            async Task after_each_async()
            {
                await SetStateAsync();
            }
        }

        [Test]
        public void class_with_both_sync_and_async_after_always_fails()
        {
            Run(typeof(WrongSpecClass));

            ExampleRunsWithInnerAsyncMismatchException("it should not know what to do");
        }
    }
}
