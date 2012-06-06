using System.Collections.Generic;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_class_level_afterAll : when_running_specs
    {
        class SpecClass : nspec
        {
            public static List<string> log;

            void after_all()
            {
                log.Add("SpecClass after_all");
            }

            void after_each()
            {
                log.Add("SpecClass after_each");
            }
        }

        class DerivedClass : SpecClass
        {
            void after_all()
            {
                log.Add("DerivedClass after_all");
            }

            void after_each()
            {
                log.Add("DerivedClass after_each");
            }

            void context_b()
            {
                beforeEach = () => log.Add("context_b beforeEach");
                it["1"] = () => log.Add("context_b it 1");
                it["2"] = () => log.Add("context_b it 2");
                afterEach = () => log.Add("context_b afterEach");
                afterAll = () => log.Add("context_b afterAll");
            }
        }

        abstract class AbstractDerivedClass1 : DerivedClass
        {
            void after_all()
            {
                log.Add("AbstractDerivedClass1 after_all");
            }
        }

        abstract class AbstractDerivedClass2 : AbstractDerivedClass1
        {
        }

        abstract class AbstractDerivedClass3 : AbstractDerivedClass2
        {
            void after_all()
            {
                log.Add("AbstractDerivedClass3 after_all");
            }
        }

        class DerivedClass2 : AbstractDerivedClass3
        {
            void after_all()
            {
                log.Add("DerivedClass2 after_all");
            }

            void running_example()
            {
                it["works"] = () => log.Add("DerivedClass2 running_example it works");
            }
        }

        [Test]
        public void after_alls_are_run_in_the_correct_order()
        {
            SpecClass.log = new List<string>(); //required to make ncrunch happy, otherwise alternating failing tests

            Run(typeof(DerivedClass));

            SpecClass.log.should_be(new[]
                {
                    "context_b beforeEach",
                    "context_b it 1",
                    "context_b afterEach",
                    "DerivedClass after_each",
                    "SpecClass after_each",
                    "context_b beforeEach",
                    "context_b it 2",
                    "context_b afterEach",
                    "DerivedClass after_each",
                    "SpecClass after_each",
                    "context_b afterAll",
                    "DerivedClass after_all",
                    "SpecClass after_all"
                });
        }

        [Test]
        [Ignore]
        public void after_alls_are_run_in_the_correct_order_when_abstract_middle_classes_are_present()
        {
            Run(typeof(DerivedClass2));

            //TODO: must implement logic for abstract classes.
            SpecClass.log.should_be(new[]
                {
                    "context_b beforeEach",
                    "context_b it 1",
                    "context_b afterEach",
                    "DerivedClass after_each",
                    "SpecClass after_each",
                    "context_b beforeEach",
                    "context_b it 2",
                    "context_b afterEach",
                    "DerivedClass after_each",
                    "SpecClass after_each",
                    "context_b afterAll",
                    "DerivedClass after_all",
                    "SpecClass after_all"
                });
        }
    }
}
