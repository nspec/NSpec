using NSpec;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_method_level_examples : describe_method_level_examples_common_cases
    {
        class AsyncSpecClass : nspec
        {
            public static bool first_async_example_executed, last_async_example_executed;

            async Task it_should_be_an_async_example()
            {
                await Utils.RunActionAsync(() =>
                {
                    first_async_example_executed = true;
                    "hello".should_be("hello");
                });
            }

            async Task it_should_be_failing_async()
            {
                await Utils.RunActionAsync(() =>
                {
                    last_async_example_executed = true;
                    "hello".should_not_be("hello");
                });
            }
        }

        [SetUp]
        public void setup()
        {
            RunWithReflector(typeof(AsyncSpecClass));
        }

        protected override bool FirstExampleExecuted { get { return AsyncSpecClass.first_async_example_executed; } }
        protected override bool LastExampleExecuted { get { return AsyncSpecClass.last_async_example_executed; } }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_wrong_method_level_examples : when_running_method_level_examples
    {
        class WrongAsyncSpecClass : nspec
        {
            async Task<long> it_should_be_failing_with_task_result()
            {
                await Utils.RunActionAsync(() => "hello".should_be("hello"));

                return -1L;
            }

            async void it_should_throw_with_async_void()
            {
                await Utils.RunActionAsync(() => "hello".should_be("hello"));
            }
        }

        [SetUp]
        public void setup()
        {
            RunWithReflector(typeof(WrongAsyncSpecClass));
        }

        [Test]
        public void async_example_with_result_should_execute()
        {
            classContext.Examples[0].HasRun.should_be_true();
        }

        [Test]
        public void async_example_with_result_should_fail()
        {
            classContext.Examples[0].Exception.should_not_be_null();
        }

        [Test]
        public void async_example_with_void_should_execute()
        {
            classContext.Examples[1].HasRun.should_be_true();
        }

        [Test]
        public void async_example_with_void_should_fail()
        {
            classContext.Examples[1].Exception.should_not_be_null();
        }
    }
}
