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
    public class describe_async_act : when_running_specs
    {
        class SpecClass : nspec
        {
            int state = 0;

            void given_async_act_is_set()
            {
                actAsync = async () =>
                {
                    state = -1;

                    await Task.Run(() => state = 1);
                };

                it["Should wait for its task to complete"] = () => state.should_be(1);
            }

            void given_async_act_fails()
            {
                actAsync = async () =>
                {
                    await Task.Run(() =>
                    {
                        throw new InvalidCastException("Some error message");
                    });
                };

                it["Should fail"] = () => true.should_be_true();
            }

            void given_both_sync_and_async_act_are_set()
            {
                act = () =>
                {
                    state = 2;
                };

                actAsync = async () =>
                {
                    state = -1;

                    await Task.Run(() => state = 1);
                };

                it["Should not know what to expect"] = () => true.should_be_true();
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
            ExampleBase example = TheExample("Should wait for its task to complete");

            example.HasRun.should_be_true();

            example.Exception.should_be_null();
        }

        [Test]
        public void async_act_with_exception_fails()
        {
            ExampleBase example = TheExample("Should fail");

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }

        [Test]
        public void context_with_both_sync_and_async_act_always_fails()
        {
            ExampleBase example = TheExample("Should not know what to expect");

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }
    }
}
