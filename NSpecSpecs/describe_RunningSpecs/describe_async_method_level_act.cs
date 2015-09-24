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
    public class describe_async_method_level_act : when_running_specs
    {
        class SpecClass : nspec
        {
            public static int state = 0;
            public static int expected = 1;

            async Task act_each()
            {
                state = -1;

                await Task.Delay(50);

                await Task.Run(() => state = 1);
            }

            void it_should_wait_for_its_task_to_complete()
            {
                state.should_be(1);
            }
        }

        [Test]
        public void async_method_level_act_waits_for_task_to_complete()
        {
            Run(typeof(SpecClass));

            ExampleBase example = TheExample("it should wait for its task to complete");

            example.HasRun.should_be_true();

            example.Exception.should_be_null();

            SpecClass.state.should_be(SpecClass.expected);
        }

        class WrongSpecClass : nspec
        {
            public int state = 0;  // public to avoid 'unused' warning

            void act_each()
            {
                state = 2;
            }

            async Task act_each_async()
            {
                state = -1;

                await Task.Run(() => state = 1);
            }

            void it_should_not_know_what_to_expect()
            {
                true.should_be_true();
            }
        }

        [Test]
        public void class_with_both_sync_and_async_act_always_fails()
        {
            Run(typeof(WrongSpecClass));

            ExampleBase example = TheExample("it should not know what to expect");

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }
    }
}
