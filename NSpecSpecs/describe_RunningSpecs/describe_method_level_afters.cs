using System;
using System.Linq;
using NSpec;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_method_level_afters : when_running_specs
    {
        class SpecClass : nspec
        {
            public static Action ContextLevelAfter = () => { };
            public static Action SubContextAfter = () => { };
            public static Func<Task> AsyncSubContextAfter = async () => { await Task.Delay(0); };

            // method- (or class-) level after
            void after_each()
            {
            }

            void method_level_context()
            {
                after = ContextLevelAfter;

                context["sub context"] = () => 
                {
                    after = SubContextAfter;

                    it["needs an example or it gets filtered"] = todo;
                };

                context["sub context with async after"] = () =>
                {
                    afterAsync = AsyncSubContextAfter;

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
        public void it_should_set_method_level_after()
        {
            // Could not find a way to actually verify that deep inside 
            // 'AfterInstance' there is a reference to 'SpecClass.after_each()'

            classContext.AfterInstance.should_not_be_null();
        }

        [Test]
        [Category("Async")]
        public void it_should_not_set_async_method_level_after()
        {
            classContext.AfterInstanceAsync.should_be_null();
        }

        [Test]
        public void it_should_set_after_on_method_level_context()
        {
            methodContext.After.should_be(SpecClass.ContextLevelAfter);
        }

        [Test]
        public void it_should_set_after_on_sub_context()
        {
            methodContext.Contexts.First().After.should_be(SpecClass.SubContextAfter);
        }

        [Test]
        [Category("Async")]
        public void it_should_set_async_after_on_sub_context()
        {
            methodContext.Contexts.Last().AfterAsync.should_be(SpecClass.AsyncSubContextAfter);
        }
    }
}
