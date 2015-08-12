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
    public class describe_async_before : when_running_specs
    {
        class SpecClass : nspec
        {
            int state = 0;

            void given_async_before_is_set()
            {
                asyncBefore = async () =>
                {
                    state = -1;

                    await RunActionAsync(() => state = 1);
                };

                it["Should wait for its task to complete"] = () => 
                    state.should_be(1);
            }

            async Task RunActionAsync(Action action)
            {
                Task fictiousAsyncOperation = Task.Run(action);

                await fictiousAsyncOperation;
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
            Example example = TheExample("Should wait for its task to complete");

            example.HasRun.should_be_true();

            example.Exception.should_be_null();
        }
    }
}
