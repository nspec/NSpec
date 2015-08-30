using System;
using System.Linq;
using NSpec;
using NUnit.Framework;
using System.Threading.Tasks;
using NSpec.Domain;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_wrong_method_level_before : when_running_specs
    {
        class WrongAsyncSpecClass : nspec
        {
            void before_each()
            {
            }

            async Task before_each_async()
            {
                await Task.Delay(0);
            }

            void it_should_not_know_what_to_expect()
            {
                true.should_be(true);
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(WrongAsyncSpecClass));
        }

        [Test]
        public void when_both_sync_and_async_are_found_it_should_fail()
        {
            ExampleBase example = TheExample("it should not know what to expect");

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_wrong_method_level_before_all : when_running_specs
    {
        class WrongAsyncSpecClass : nspec
        {
            void before_all()
            {
            }

            async Task before_all_async()
            {
                await Task.Delay(0);
            }

            void it_should_not_know_what_to_expect()
            {
                true.should_be(true);
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(WrongAsyncSpecClass));
        }

        [Test]
        public void when_both_sync_and_async_are_found_it_should_fail()
        {
            ExampleBase example = TheExample("it should not know what to expect");

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }
    }

    // describe_async_wrong_method_level_after TODO-ASYNC
    // describe_async_wrong_method_level_after_all TODO-ASYNC
}
