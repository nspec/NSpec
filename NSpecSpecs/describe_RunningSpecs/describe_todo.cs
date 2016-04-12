using System;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class using_xit : describe_todo
    {
        class XitClass : nspec
        {
            void method_level_context()
            {
                xit["should be pending"] = () => { };
            }
        }

        [Test]
        public void example_should_be_pending()
        {
            ExampleFrom(typeof(XitClass)).Pending.should_be_true();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class using_async_xit : describe_todo
    {
        class AsyncXitClass : nspec
        {
            void method_level_context()
            {
                xitAsync["should be pending"] = async () => await Task.Run(() => { });
            }
        }

        [Test]
        public void example_should_be_pending()
        {
            ExampleFrom(typeof(AsyncXitClass)).Pending.should_be_true();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class using_async_lambda_with_xit : describe_todo
    {
        class AsyncLambdaClass : nspec
        {
            void method_level_context()
            {
                xit["should fail because xit is set to async lambda"] = async () => await Task.Run(() => { });

                // No chance of error when (async) return value is explicitly typed. The following do not even compile:
                /*
                Func<Task> asyncTaggedDelegate = async () => await Task.Run(() => { });
                Func<Task> asyncUntaggedDelegate = () => { return Task.Run(() => { }); };

                it["Should fail because xit is set to async tagged delegate"] = asyncTaggedDelegate;

                it["Should fail because xit is set to async untagged delegate"] = asyncUntaggedDelegate;
                */
            }
        }

        [Test]
        public void sync_pending_example_set_to_async_lambda_fails()
        {
            var example = ExampleFrom(typeof(AsyncLambdaClass));

            example.HasRun.should_be_true();

            example.Exception.should_not_be_null();

            example.Pending.should_be_true();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class using_todo : describe_todo
    {
        class TodoClass : nspec
        {
            void method_level_context()
            {
                it["should be pending"] = todo;
            }
        }

        [Test]
        public void example_should_be_pending()
        {
            ExampleFrom(typeof(TodoClass)).Pending.should_be_true();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class using_async_todo : describe_todo
    {
        class AsyncTodoClass : nspec
        {
            void method_level_context()
            {
                itAsync["should be pending"] = todoAsync;
            }
        }

        [Test]
        public void example_should_be_pending()
        {
            ExampleFrom(typeof(AsyncTodoClass)).Pending.should_be_true();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class using_todo_with_throwing_before : describe_todo
    {
        class TodoClass : nspec
        {
            void method_level_context()
            {
                before = () => { throw new Exception(); };
                it["should be pending"] = todo;
            }
        }

        [Test]
        public void example_should_not_fail_but_be_pending()
        {
            ExampleFrom(typeof(TodoClass)).Pending.should_be_true();
        }
    }

    public class describe_todo : when_running_specs
    {
        protected ExampleBase ExampleFrom(Type type)
        {
            Run(type);

            return classContext.AllExamples().First();
        }
    }
}
