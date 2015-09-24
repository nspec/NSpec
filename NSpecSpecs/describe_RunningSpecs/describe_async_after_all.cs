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
    public class describe_async_after_all : when_running_specs
    {
        class SpecClass : nspec
        {
            public static int state = 0;
            public static int expected = 1;

            void given_async_after_is_set()
            {
                it["Should have a specification"] = () => state.should_be(0);

                afterAllAsync = async () =>
                {
                    state = -1;

                    await Task.Delay(50);

                    await Task.Run(() => state = 1);
                };
            }

            void given_async_after_all_fails()
            {
                afterAllAsync = async () =>
                {
                    await Task.Run(() =>
                    {
                        throw new InvalidCastException("Some error message");
                    });
                };

                it["Should fail"] = () => true.should_be_true();
            }

            void given_both_sync_and_async_after_all_are_set()
            {
                afterAll = () =>
                {
                    state = 2;
                };

                afterAllAsync = async () =>
                {
                    state = -1;

                    await Task.Run(() => state = 1);
                };

                it["Should not know what to do"] = () => true.should_be_true();
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
            ExampleBase example = TheExample("Should have a specification");

            example.HasRun.should_be_true();

            example.Exception.should_be_null();

            SpecClass.state.should_be(SpecClass.expected);
        }

        [Test]
        [Ignore("Until 'AfterAlls' exceptions are not registered")]
        public void async_after_all_with_exception_fails()
        {
            ExampleBase example = TheExample("Should fail");

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }

        [Test]
        [Ignore("Until 'AfterAlls' exceptions are not registered")]
        public void context_with_both_sync_and_async_after_all_always_fails()
        {
            ExampleBase example = TheExample("Should not know what to do");

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }
    }
}
