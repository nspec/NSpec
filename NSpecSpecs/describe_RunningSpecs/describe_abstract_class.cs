using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpecSpecs.WhenRunningSpecs;
using NSpec;
using NSpec.Domain;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    public class describe_abstract_classes : when_running_specs
    {
        abstract class Abstract1 : nspec
        {
            public string be = "", ac = "", af = "", all = "";

            void abstract1_example()
            {
                it["abstract1 tests nothing", "abstract1"] = () => all += "i1";
            }

            void before_each()
            {
                be += "1";
                all += "b1";
            }

            void act_each()
            {
                ac += "1";
                all += "ac1";
            }

            void after_each()
            {
                af += "1";
                all += "af1";
            }
        }

        class Concrete2 : Abstract1
        {
            void concrete2_example()
            {
                it["concrete2 tests nothing", "concrete2"] = () => all += "i2";
            }

            void before_each()
            {
                be += "2";
                all += "b2";
            }

            void act_each()
            {
                ac += "2";
                all += "ac2";
            }

            void after_each()
            {
                af += "2";
                all += "af2";
            }
        }

        abstract class Abstract3 : Concrete2
        {
            void abstract3_example()
            {
                it["abstract3 tests nothing", "abstract3"] = () => all += "i3";
            }

            void before_each()
            {
                be += "3";
                all += "b3";
            }

            void act_each()
            {
                ac += "3";
                all += "ac3";
            }

            void after_each()
            {
                af += "3";
                all += "af3";
            }
        }

        abstract class Abstract4 : Abstract3
        {
            void abstract4_example()
            {
                it["abstract4 tests nothing", "abstract4"] = () => all += "i4";
            }

            void before_each()
            {
                be += "4";
                all += "b4";
            }

            void act_each()
            {
                ac += "4";
                all += "ac4";
            }

            void after_each()
            {
                af += "4";
                all += "af4";
            }
        }

        class Concrete5 : Abstract4
        {
            void concrete5_example()
            {
                it["concrete5 tests nothing", "concrete5"] = () => all += "i5";
            }

            void before_each()
            {
                be += "5";
                all += "b5";
            }

            void act_each()
            {
                ac += "5";
                all += "ac5";
            }

            void after_each()
            {
                af = "5";
                all += "af5";
            }
        }

        [Test]
        public void abstracts_should_not_be_added_as_class_contexts()
        {
            Run(new[] { typeof(Concrete5), typeof(Abstract1), typeof(Abstract3), typeof(Abstract4) });

            var allClassContexts =
                contextCollection[0].AllContexts().Where(c => c.GetType() == typeof(ClassContext)).ToList();

            allClassContexts.should_contain(c => c.Name.EndsWith("Concrete2"));
            allClassContexts.should_contain(c => c.Name.EndsWith("Concrete5"));
            allClassContexts.should_not_contain(c => c.Name.Contains("Abstract"));
        }

        [Test]
        public void abstracts_should_not_load_any_contexts_on_their_own()
        {
            Run(new[] { typeof(Abstract1), typeof(Abstract3), typeof(Abstract4) });

            contextCollection.Count.should_be(0);
        }

        [Test,
         TestCase(typeof(Concrete2), new[] { "abstract1 example" }),
         TestCase(typeof(Concrete5), new[] { "abstract3 example", "abstract4 example" })]
        public void contexts_in_abstracts_should_be_added_under_derived_concrete_class_context(
            Type type, string[] expectedMethodContextsFromAbstracts)
        {
            Run(type);

            Context currentClassContext = TheContext(type.Name);

            foreach (string methodContextName in expectedMethodContextsFromAbstracts)
            {
                string name = methodContextName;
                currentClassContext.Contexts.should_contain(c => c.Name == name);
            }
        }

        [Test]
        public void contexts_in_not_immediate_abstract_bases_should_not_be_under_derived_concrete_class_context()
        {
            Run(typeof(Concrete5));

            TheContext("Concrete5").Contexts.should_not_contain(c => c.Name == "abstract1 example");
        }

        [Test(Description = "before_each() in concrete classes affects base abstracts"),
         TestCase(typeof(Concrete2), "abstract1", "12"),
         TestCase(typeof(Concrete2), "concrete2", "12"),
         TestCase(typeof(Concrete5), "abstract3", "12345"),
         TestCase(typeof(Concrete5), "abstract4", "12345"),
         TestCase(typeof(Concrete5), "concrete5", "12345")]
        public void before_eaches_should_run_in_the_correct_order(Type type, string tags, string expectedBe)
        {
            Run(type, tags);

            var specInstance = classContext.GetInstance() as Abstract1;

            specInstance.should_not_be_null();
            specInstance.be.should_be(expectedBe);
        }

        [Test(Description = "act_each() in concrete classes affects base abstracts"),
         TestCase(typeof(Concrete2), "abstract1", "12"),
         TestCase(typeof(Concrete2), "concrete2", "12"),
         TestCase(typeof(Concrete5), "abstract3", "12345"),
         TestCase(typeof(Concrete5), "abstract4", "12345"),
         TestCase(typeof(Concrete5), "concrete5", "12345")]
        public void act_eaches_should_run_in_the_correct_order(Type type, string tags, string expectedAc)
        {
            Run(type, tags);

            var specInstance = classContext.GetInstance() as Abstract1;

            specInstance.should_not_be_null();
            specInstance.ac.should_be(expectedAc);
        }

        [Test(Description = "after_each() in concrete classes affects base abstracts"),
         TestCase(typeof(Concrete2), "abstract1", "21"),
         TestCase(typeof(Concrete2), "concrete2", "21"),
         TestCase(typeof(Concrete5), "abstract3", "54321"),
         TestCase(typeof(Concrete5), "abstract4", "54321"),
         TestCase(typeof(Concrete5), "concrete5", "54321")]
        public void after_eaches_should_run_in_the_correct_order(Type type, string tags, string expectedAf)
        {
            Run(type, tags);

            var specInstance = classContext.GetInstance() as Abstract1;

            specInstance.should_not_be_null();
            specInstance.af.should_be(expectedAf);
        }

        [Test,
         TestCase(typeof(Concrete2), "abstract1", "b1b2ac1ac2i1af2af1"),
         TestCase(typeof(Concrete2), "concrete2", "b1b2ac1ac2i2af2af1"),
         TestCase(typeof(Concrete5), "abstract3", "b1b2b3b4b5ac1ac2ac3ac4ac5i3af5af4af3af2af1"),
         TestCase(typeof(Concrete5), "abstract4", "b1b2b3b4b5ac1ac2ac3ac4ac5i4af5af4af3af2af1"),
         TestCase(typeof(Concrete5), "concrete5", "b1b2b3b4b5ac1ac2ac3ac4ac5i5af5af4af3af2af1")]
        public void execution_should_run_in_the_correct_order(Type type, string tags, string expectedAll)
        {
            Run(type, tags);

            var specInstance = classContext.GetInstance() as Abstract1;

            specInstance.should_not_be_null();
            specInstance.all.should_be(expectedAll);
        }
    }
}
