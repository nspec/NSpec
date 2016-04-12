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
                    throw new InvalidCastException("Some error message");
                });
            }

            protected void ShouldHaveInitialState()
            {
                state.should_be(0);
            }

            protected void ShouldHaveFinalState()
            {
                state.should_be(1);
            }

            protected void PassAlways()
            {
                true.should_be_true();
            }
        }

        protected void ExampleRunsWithExpectedState(string name)
        {
            ExampleBase example = TheExample(name);

            example.HasRun.should_be_true();

            example.Exception.should_be_null();

            BaseSpecClass.state.should_be(BaseSpecClass.expected);
        }

        protected void ExampleRunsWithException(string name)
        {
            ExampleBase example = TheExample(name);

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }
    }
}
