using System.Collections.Generic;
using NUnit.Framework;
using NSpec;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    public class describe_class_level_before : when_running_specs
    {
        class SpecClass : nspec
        {
            List<int> ints = null;

            void before_each()
            {
                ints = new List<int>();
            }

            void list_manipulations()
            {
                it["should not be null"] = () => ints.Count.should_be(0);

                context["number in collection"] = () =>
                {
                    before = () => ints.Add(15);

                    it["should have one record"] = () => ints.Count.should_be(1);
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            Init(typeof(SpecClass));

            Run();
        }

        [Test]
        public void should_run_class_level_before_then_method_level_before()
        {
            TheExample("should not be null").should_not_have_failed();
        }

        [Test]
        public void should_run_example_within_a_sub_context()
        {
            TheExample("should have one record").should_not_have_failed();
        }
    }
}
