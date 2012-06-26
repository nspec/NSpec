using System.Collections.Generic;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    class describe_class_level_before_for_abstract_class : when_running_specs
    {
        class BaseClass : nspec
        {
            protected List<int> ints;
            
            void before_each()
            {
                ints = new List<int>();

                ints.Add(1);
            }

            void list_manipulations()
            {
                it["should contain 1"] = () => ints.should_contain(1);

                it["should have one record too"] = () => ints.Count.should_be(1);
            }
        }

        abstract class DerivedClass1 : BaseClass
        {
            void before_each()
            {
                ints.Add(2);
            }

            void list_manipulations()
            {
                it["should contain 2"] = () => ints.should_contain(2);

                it["should have three records"] = () => ints.Count.should_be(3);
            }
        }

        abstract class DerivedClass2 : DerivedClass1 {}

        class DerivedClass3 : DerivedClass2
        {
            void before_each()
            {
                ints.Add(3);
            }

            void list_manipulations()
            {
                it["should contain 2 too"] = () => ints.should_contain(2);

                it["should contain 3"] = () => ints.should_contain(3);

                it["should have three records too"] = () => ints.Count.should_be(3);
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(DerivedClass3));
        }

        [Test]
        public void should_run_example_within_a_sub_context_in_a_derived_class()
        {
            TheExample("should contain 1").should_have_passed();

            TheExample("should have one record too").should_have_passed();
        }

        [Test]
        public void it_runs_examples_from_abstract_class_as_if_they_belonged_to_concrete_class()
        {
            TheExample("should contain 2").should_have_passed();

            TheExample("should contain 2 too").should_have_passed();

            TheExample("should contain 3").should_have_passed();

            TheExample("should have three records too").should_have_passed();
        }
    }
}
