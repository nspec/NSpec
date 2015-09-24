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
    public class describe_async_before_all : when_describing_async_hooks
    {
        class SpecClass : BaseSpecClass
        {
            void given_async_before_all_is_set()
            {
                beforeAllAsync = SetStateAsync;

                it["Should have final value"] = ShouldHaveFinalState;
            }

            void given_async_before_all_fails()
            {
                beforeAllAsync = FailAsync;

                it["Should fail"] = PassAlways;
            }

            void given_both_sync_and_async_before_all_are_set()
            {
                beforeAll = SetAnotherState;

                beforeAllAsync = SetStateAsync;

                it["Should not know what to expect"] = PassAlways;
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void async_before_all_waits_for_task_to_complete()
        {
            async_hook_waits_for_task_to_complete("Should have final value");
        }

        [Test]
        public void async_before_all_with_exception_fails()
        {
            async_hook_with_exception_fails("Should fail");
        }

        [Test]
        public void context_with_both_sync_and_async_before_always_fails()
        {
            context_with_both_sync_and_async_hook_always_fails("Should not know what to expect");
        }
    }
}
