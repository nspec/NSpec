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
