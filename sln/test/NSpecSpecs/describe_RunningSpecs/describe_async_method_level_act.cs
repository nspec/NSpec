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
    public class describe_async_method_level_act : when_describing_async_hooks
    {
        class SpecClass : BaseSpecClass
        {
            async Task act_each()
            {
                await SetStateAsync();
            }

            void it_should_wait_for_its_task_to_complete()
            {
                ShouldHaveFinalState();
            }
        }

        [Test]
        public void async_method_level_act_waits_for_task_to_complete()
        {
            Run(typeof(SpecClass));

            ExampleRunsWithExpectedState("it should wait for its task to complete");
        }

        class WrongSpecClass : BaseSpecClass
        {
            void act_each()
            {
                SetAnotherState();
            }

            async Task act_each_async()
            {
                await SetStateAsync();
            }

            void it_should_not_know_what_to_expect()
            {
                Assert.That(true, Is.True);
            }
        }

        [Test]
        public void class_with_both_sync_and_async_act_always_fails()
        {
            Run(typeof(WrongSpecClass));

            ExampleRunsWithInnerAsyncMismatchException("it should not know what to expect");
        }
    }
}
