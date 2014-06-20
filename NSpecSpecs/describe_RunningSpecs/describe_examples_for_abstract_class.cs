using System.Collections.Generic;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using NSpec.Assertions.nUnit;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    class describe_examples_for_abstract_class : when_running_specs
    {
        class Base : nspec
        {
            protected List<int> ints;
            
            void before_each()
            {
                ints = new List<int>();

                ints.Add(1);
            }

            void list_manipulations()
            {
                it["should be 1"] = () => ints.should_be(1);
            }
        }

        abstract class Abstract : Base
        {
            void before_each()
            {
                ints.Add(2);
            }

            void list_manipulations()
            {
                //since abstract classes can only run in derived concrete context classes
                //the context isn't quite what you might expect.
                it["should be 1, 2, 3"] = () => ints.should_be(1, 2, 3);
            }
        }

        class Concrete : Abstract
        {
            void before_each()
            {
                ints.Add(3);
            }

            void list_manipulations()
            {
                it["should be 1, 2, 3 too"] = () => ints.should_be(1, 2, 3);
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(Concrete));
        }

        [Test]
        public void should_run_example_within_a_sub_context_in_a_derived_class()
        {
            TheExample("should be 1").should_have_passed();
        }

        [Test]
        public void it_runs_examples_from_abstract_class_as_if_they_belonged_to_concrete_class()
        {
            TheExample("should be 1, 2, 3").should_have_passed();

            TheExample("should be 1, 2, 3 too").should_have_passed();
        }
    }
}
