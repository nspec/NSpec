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
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void async_example_waits_for_task_to_complete()
        {
            async_hook_waits_for_task_to_complete("Should wait for its task to complete");
        }

        [Test]
        public void async_example_with_exception_fails()
        {
            async_hook_with_exception_fails("Should fail asynchronously");
        }
    }
}
