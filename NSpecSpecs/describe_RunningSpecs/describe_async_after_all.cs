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
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void async_after_all_waits_for_task_to_complete()
        {
            async_hook_waits_for_task_to_complete("Should have initial value");
        }

        [Test]
        [Ignore("Until 'AfterAlls' exceptions are not registered")]
        public void async_after_all_with_exception_fails()
        {
            async_hook_with_exception_fails("Should fail");
        }

        [Test]
        [Ignore("Until 'AfterAlls' exceptions are not registered")]
        public void context_with_both_sync_and_async_after_all_always_fails()
        {
            context_with_both_sync_and_async_hook_always_fails("Should not know what to do");
        }
    }
}
