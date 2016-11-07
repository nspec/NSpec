using System;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using System.Threading.Tasks;
using NSpecSpecs.describe_RunningSpecs.Exceptions;
using FluentAssertions;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Pending")]
    public class using_xit : describe_todo
    {
        class XitClass : nspec
        {
            void method_level_context()
            {
                xit["should be pending"] = () => { executed = true; };
            }

            public static bool executed = false;
        }

        [Test]
        public void example_should_be_pending()
        {
            var example = ExampleFrom(typeof(XitClass));

            example.Pending.Should().BeTrue();
        }

        [Test]
        public void example_should_not_throw()
        {
            var example = ExampleFrom(typeof(XitClass));

            example.Exception.Should().BeNull();
        }

        [Test]
        public void example_body_should_not_run()
        {
            XitClass.executed.Should().BeFalse();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Pending")]
    [Category("Async")]
    public class using_async_xit : describe_todo
    {
        class AsyncXitClass : nspec
        {
            void method_level_context()
            {
                xitAsync["should be pending"] = async () =>
                {
                    executed = true;
                    await Task.Run(() => { });
                };
            }

            public static bool executed = false;
        }

        [Test]
        public void example_should_be_pending()
        {
            var example = ExampleFrom(typeof(AsyncXitClass));

            example.HasRun.Should().BeTrue();

            example.Pending.Should().BeTrue();
        }

        [Test]
        public void example_should_not_throw()
        {
            var example = ExampleFrom(typeof(AsyncXitClass));

            example.Exception.Should().BeNull();
        }

        [Test]
        public void example_body_should_not_run()
        {
            AsyncXitClass.executed.Should().BeFalse();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Pending")]
    [Category("Async")]
    public class using_xit_with_async_lambda : describe_todo
    {
        class XitClassWithAsyncLambda : nspec
        {
            void method_level_context()
            {
                xit["should fail because xit is set to async lambda"] = async () =>
                {
                    executed = false;
                    await Task.Run(() => { });
                };

                // No chance of error when (async) return value is explicitly typed. The following do not even compile:
                /*
                Func<Task> asyncTaggedDelegate = async () => await Task.Run(() => { });
                Func<Task> asyncUntaggedDelegate = () => { return Task.Run(() => { }); };

                xit["Should fail because xit is set to async tagged delegate"] = asyncTaggedDelegate;

                xit["Should fail because xit is set to async untagged delegate"] = asyncUntaggedDelegate;
                */
            }

            public static bool executed = false;
        }

        [Test]
        public void example_should_be_pending()
        {
            var example = ExampleFrom(typeof(XitClassWithAsyncLambda));

            example.HasRun.Should().BeTrue();

            example.Pending.Should().BeTrue();
        }

        [Test]
        public void example_should_throw()
        {
            var example = ExampleFrom(typeof(XitClassWithAsyncLambda));

            example.Exception.Should().NotBeNull();

            example.Exception.Should().BeOfType<AsyncMismatchException>();
        }

        [Test]
        public void example_body_should_not_run()
        {
            XitClassWithAsyncLambda.executed.Should().BeFalse();
        }
    }

    /*
     * Test case on using async xit with sync lambda cannot be performed,
     * as setting xitAsync to a sync lambda does not even compile:
     *
     * xitAsync["should fail because xit is set to sync lambda"] = () => { executed = false; };
     *
     */

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Pending")]
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
            var example = ExampleFrom(typeof(TodoClass));

            example.HasRun.Should().BeTrue();

            example.Pending.Should().BeTrue();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Pending")]
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
            var example = ExampleFrom(typeof(AsyncTodoClass));

            example.HasRun.Should().BeTrue();

            example.Pending.Should().BeTrue();
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Pending")]
    public class using_todo_with_throwing_before : describe_todo
    {
        class TodoClass : nspec
        {
            void method_level_context()
            {
                before = () => { throw new KnownException(); };

                it["should be pending"] = todo;
            }
        }

        [Test]
        public void example_should_be_pending()
        {
            var example = ExampleFrom(typeof(TodoClass));

            example.HasRun.Should().BeTrue();

            example.Pending.Should().BeTrue();
        }

        [Test]
        public void example_should_not_throw()
        {
            var example = ExampleFrom(typeof(TodoClass));

            example.Exception.Should().BeNull();
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
