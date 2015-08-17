using System;
using System.Linq;
using NSpec;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    [Category("Async")]
    public class describe_async_method_level_befores : when_running_specs
    {
        class SpecClass : nspec
        {
            public static Func<Task> ContextLevelBefore = async () => { await Task.Delay(0); };
            public static Action SubContextBefore = () => { };
            public static Func<Task> AsyncSubContextBefore = async () => { await Task.Delay(0); };

            async Task before_each() 
            { 
                await Task.Delay(0); 
            }

            void method_level_context()
            {
                asyncBefore = ContextLevelBefore;

                context["sub context"] = () => 
                {
                    before = SubContextBefore;

                    it["needs an example or it gets filtered"] = todo;
                };

                context["sub context with async before"] = () =>
                {
                    asyncBefore = AsyncSubContextBefore;

                    it["needs another example or it gets filtered"] = todo;
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void it_should_set_async_method_level_before()
        {
            // Could not find a way to actually verify that deep inside 
            // 'AsyncBeforeInstance' there is a reference to 'SpecClass.before_each()'

            classContext.AsyncBeforeInstance.should_not_be_null();
        }

        [Test]
        public void it_should_not_set_method_level_before()
        {
            classContext.BeforeInstance.should_be_null();
        }

        [Test]
        public void it_should_set_async_before_on_method_level_context()
        {
            methodContext.AsyncBefore.should_be(SpecClass.ContextLevelBefore);
        }

        [Test]
        public void it_should_set_before_on_sub_context()
        {
            methodContext.Contexts.First().Before.should_be(SpecClass.SubContextBefore);
        }

        [Test]
        public void it_should_set_async_before_on_sub_context()
        {
            methodContext.Contexts.Last().AsyncBefore.should_be(SpecClass.AsyncSubContextBefore);
        }
    }
}
