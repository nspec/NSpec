using FluentAssertions;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.describe_RunningSpecs.Exceptions;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.describe_RunningSpecs
{
    public abstract class when_describing_async_hooks : when_running_specs
    {
        protected class BaseSpecClass : nspec
        {
            public static int state = -2;
            public static int expected = 1;

            public BaseSpecClass()
            {
                state = 0;
            }

            protected async Task SetStateAsync()
            {
                state = -1;

                await Task.Delay(50);

                await Task.Run(() => state = 1);
            }

            protected void SetAnotherState()
            {
                state = 2;
            }

            protected async Task FailAsync()
            {
                await Task.Run(() =>
                {
                    throw new KnownException("Some error message");
                });
            }

            protected void ShouldHaveInitialState()
            {
                Assert.That(state, Is.EqualTo(0));
            }

            protected void ShouldHaveFinalState()
            {
                Assert.That(state, Is.EqualTo(1));
            }
        }

        protected void ExampleRunsWithExpectedState(string name)
        {
            ExampleBase example = TheExample(name);

            example.HasRun.Should().BeTrue();

            example.Exception.Should().BeNull();

            BaseSpecClass.state.Should().Be(BaseSpecClass.expected);
        }

        protected void ExampleRunsWithException(string name)
        {
            ExampleBase example = TheExample(name);

            example.HasRun.Should().BeTrue();

            example.Exception.Should().NotBeNull();
        }

        protected void ExampleRunsWithAsyncMismatchException(string name)
        {
            ExampleBase example = TheExample(name);

            example.HasRun.Should().BeTrue();

            example.Exception.Should().NotBeNull();

            example.Exception.GetType().Should().Be(typeof(AsyncMismatchException));
        }

        protected void ExampleRunsWithInnerAsyncMismatchException(string name)
        {
            ExampleBase example = TheExample(name);

            example.HasRun.Should().BeTrue();

            example.Exception.Should().NotBeNull();

            example.Exception.InnerException.Should().NotBeNull();

            example.Exception.InnerException.GetType().Should().Be(typeof(AsyncMismatchException));
        }
    }
}
