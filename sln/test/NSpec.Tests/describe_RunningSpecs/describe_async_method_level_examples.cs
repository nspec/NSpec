using FluentAssertions;
using NSpec.Domain;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpec.Tests.WhenRunningSpecs
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

                    Assert.That("hello", Is.EqualTo("hello"));
                });
            }

            async Task it_should_be_failing_async()
            {
                await Task.Run(() =>
                {
                    last_async_example_executed = true;

                    Assert.That("hello", Is.Not.EqualTo("hello"));
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
                await Task.Run(() => Assert.That("hello", Is.EqualTo("hello")));

                return -1L;
            }

            async void it_should_throw_with_async_void()
            {
                await Task.Run(() => Assert.That("hello", Is.EqualTo("hello")));
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

            example.Exception.Should().BeOfType<AsyncMismatchException>();
        }

        [Test]
        public void async_example_with_void_should_fail()
        {
            var example = classContext.Examples[1];

            example.HasRun.Should().BeTrue();

            example.Exception.Should().NotBeNull();

            example.Exception.Should().BeOfType<AsyncMismatchException>();
        }
    }
}
