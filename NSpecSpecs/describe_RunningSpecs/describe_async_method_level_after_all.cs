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
    public class describe_async_method_level_after_all : when_running_specs
    {
        class SpecClass : nspec
        {
            public static int state = 0;

            async Task after_all()
            {
                state = -1;

                await Task.Run(() => state = 1);
            }

            void it_should_have_some_spec()
            {
                state.should_be(0);
            }
        }

        [Test]
        public void async_method_level_after_all_waits_for_task_to_complete()
        {
            Run(typeof(SpecClass));

            SpecClass.state.should_be(1);

            ExampleBase example = TheExample("it should have some spec");

            example.HasRun.should_be_true();

            example.Exception.should_be_null();
        }

        class WrongSpecClass : nspec
        {
            int state = 0;

            void after_all()
            {
                state = 2;
            }

            async Task after_all_async()
            {
                state = -1;

                await Task.Run(() => state = 1);
            }

            void it_should_not_know_what_to_do()
            {
                state.should_be(0);
            }
        }

        [Test]
        [Ignore("Until 'AfterAlls' exceptions are not registered")]
        public void class_with_both_sync_and_async_after_all_always_fails()
        {
            Run(typeof(WrongSpecClass));

            ExampleBase example = TheExample("it should not know what to do");

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }
    }
}
