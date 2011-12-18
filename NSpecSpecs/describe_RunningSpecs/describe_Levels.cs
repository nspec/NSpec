using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    public class describe_Levels : when_running_specs
    {
        class describe_numbers : nspec
        {
            void method_level_context()
            {
                it["1 is 1"] = () => 1.Is(1);

                context["except in crazy world"] = () =>
                {
                    it["1 is 2"] = () => 1.Is(2);
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            Init(typeof(describe_numbers));

            Run();
        }

        [Test]
        public void classes_that_directly_inherit_nspec_have_level_1()
        {
            TheContext("describe numbers").Level.Is(1);
        }

        [Test]
        public void method_level_contexts_have_one_level_deeper()
        {
            TheContext("method level context").Level.Is(2);
        }

        [Test]
        public void and_nested_contexts_one_more_deep()
        {
            TheContext("except in crazy world").Level.Is(3);
        }
    }
}