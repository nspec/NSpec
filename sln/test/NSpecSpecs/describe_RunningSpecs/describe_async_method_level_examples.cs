using FluentAssertions;
using NSpec;
using NSpec.Domain;
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
                await Task.Run(() =>
                {
                    first_async_example_executed = true;
                    "hello".Should().Be("hello");
                });
            }

            async Task it_should_be_failing_async()
            {
                await Task.Run(() =>
                {
                    last_async_example_executed = true;
                    "hello".Should().NotBe("hello");
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
                await Task.Run(() => "hello".Should().Be("hello"));

                return -1L;
            }

            async void it_should_throw_with_async_void()
            {
                await Task.Run(() => "hello".Should().Be("hello"));
            }
        }

        [SetUp]
        public void setup()
        {
            RunWithReflector(typeof(WrongAsyncSpecClass));
        }

        [Test]
        public void async_example_with_result_should_fail()
        {
            var example = classContext.Examples[0];

            example.HasRun.Should().BeTrue();

            example.Exception.Should().NotBeNull();

            example.Exception.GetType().Should().Be(typeof(AsyncMismatchException));
        }

        [Test]
        public void async_example_with_void_should_fail()
        {
            var example = classContext.Examples[1];

            example.HasRun.Should().BeTrue();

            example.Exception.Should().NotBeNull();

            example.Exception.GetType().Should().Be(typeof(AsyncMismatchException));
        }
    }
}
