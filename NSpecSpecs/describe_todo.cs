using System;
using System.Linq;
using NSpec.Domain.Extensions;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;

namespace NSpecNUnit
{
    [TestFixture]
    public class using_todo : describe_todo
    {
        class TodoClass : nspec
        {
            public void method_level_context()
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
            public void method_level_context()
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

    public class describe_todo
    {
        protected Example ExampleFrom(Type type)
        {
            var classContext = new Context(type);

            var methodContext = new Context(type.Methods().First());

            classContext.AddContext(methodContext);

            classContext.Run();

            return classContext.AllExamples().First();
        }
    }
}
