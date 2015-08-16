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
    public class describe_async_example : when_running_specs
    {
        class SpecClass : nspec
        {
            int state = 0;

            void given_async_example_is_set()
            {
                asyncIt["Should wait for its task to complete"] = async () =>
                {
                    state = -1;

                    await Utils.RunActionAsync(() => state = 1);

                    state.should_be(1);
                };
            }

            void given_async_example_fails()
            {
                asyncIt["Should fail asynchronously"] = async () =>
                {
                    await Utils.RunActionAsync(() => 
                    { 
                        throw new InvalidCastException("Some error message"); 
                    });
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void async_example_waits_for_task_to_complete()
        {
            ExampleBase example = TheExample("Should wait for its task to complete");
            
            example.HasRun.should_be_true();

            example.Exception.should_be_null();
        }

        [Test]
        public void async_example_with_exception_fails()
        {
            ExampleBase example = TheExample("Should fail asynchronously");

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();
        }
    }
}
