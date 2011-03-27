using System;
using System.Linq;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
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

    public class describe_todo : when_running_specs
    {
        protected Example ExampleFrom(Type type)
        {
            Run(type);

            return classContext.AllExamples().First();
        }
    }
}
