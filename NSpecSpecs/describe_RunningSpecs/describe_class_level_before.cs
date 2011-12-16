using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_class_level_before : when_running_specs
    {
        class SpecClass : nspec
        {
            protected List<int> ints = null;

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

        class DerivedClass1 : SpecClass
        {
            void before_each()
            {
                ints.Add(1);
            }

            void list_manipulations()
            {
                it["should contain 1"] = () => ints.should_contain(1);
                it["should have one record too"] = () => ints.Count.should_be(1);
            }
        }

        abstract class AbstractDerivedClass1 : DerivedClass1
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

        abstract class AbstractDerivedClass2 : AbstractDerivedClass1
        {
        }

        class DerivedClass2 : AbstractDerivedClass2
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

        [Test,
         TestCase(typeof(SpecClass)),
         TestCase(typeof(DerivedClass1)),
         TestCase(typeof(DerivedClass2))]
        public void should_run_class_level_before_then_method_level_before(Type typeToRun)
        {
            Run(typeToRun);
            TheExample("should not be null").ExampleLevelException.should_be(null);
        }

        [Test,
         TestCase(typeof(SpecClass))]
        public void should_run_example_within_a_sub_context(Type typeToRun)
        {
            Run(typeToRun);
            TheExample("should have one record").ExampleLevelException.should_be(null);
        }

        [Test,
         TestCase(typeof(DerivedClass1)),
         TestCase(typeof(DerivedClass2))]
        public void befores_in_derived_classes_should_not_affect_concrete_base_class(Type typeToRun)
        {
            Run(typeToRun);
            TheExample("should have one record").ExampleLevelException.should_be(null);
        }

        [Test,
         TestCase(typeof(DerivedClass1)),
         TestCase(typeof(DerivedClass2))]
        public void should_run_example_within_a_sub_context_in_a_derived_class(Type typeToRun)
        {
            Run(typeToRun);
            TheExample("should contain 1").ExampleLevelException.should_be(null);
            TheExample("should have one record too").ExampleLevelException.should_be(null);
        }

        [Test,
         TestCase(typeof(DerivedClass2))]
        public void should_run_befores_in_abstract_base_classes(Type typeToRun)
        {
            Run(typeToRun);
            TheExample("should contain 2").ExampleLevelException.should_be(null);
            TheExample("should contain 2 too").ExampleLevelException.should_be(null);
            TheExample("should contain 3").ExampleLevelException.should_be(null);
            TheExample("should have three records too").ExampleLevelException.should_be(null);
        }

        [Test,
         TestCase(typeof(DerivedClass2))]
        public void befores_in_derived_classes_should_affect_abstract_base_classes(Type typeToRun)
        {
            Run(typeToRun);
            TheExample("should have three records").ExampleLevelException.should_be(null);
        }
    }
}
