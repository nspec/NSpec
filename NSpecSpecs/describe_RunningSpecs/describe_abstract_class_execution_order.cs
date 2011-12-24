using System;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    public class describe_abstract_class_execution_order : when_running_specs
    {
        abstract class Class1 : nspec
        {
            public string beforeExecutionOrder = "", actExecutionOrder = "", afterExecutionOrder = "", allExecutions = "";

            public void LogBefore(string classId)
            {
                beforeExecutionOrder += classId;
                allExecutions += "b" + classId;
            }

            public void LogAct(string classId)
            {
                actExecutionOrder += classId;
                allExecutions += "ac" + classId;
            }

            public void LogAfter(string classId)
            {
                afterExecutionOrder += classId;
                allExecutions += "af" + classId;
            }

            public void LogExample(string classId)
            {
                allExecutions += "i" + classId;
            }

            void abstract1_example()
            {
                it["abstract1 tests nothing", "example_in_abtract_class"] = () => LogExample(classId: "1");
            }

            void before_each()
            {
                LogBefore(classId: "1");
            }

            void act_each()
            {
                LogAct(classId: "1");
            }

            void after_each()
            {
                LogAfter(classId: "1");
            }
        }

        class Class2 : Class1
        {
            void concrete2_example()
            {
                it["concrete2 tests nothing", "example_in_concrete_class_that_inherits_abstract"] = () => LogExample(classId: "2");
            }

            void before_each()
            {
                LogBefore(classId: "2");
            }

            void act_each()
            {
                LogAct(classId: "2");
            }

            void after_each()
            {
                LogAfter(classId: "2");
            }
        }

        abstract class Class3 : Class2
        {
            void abstract3_example()
            {
                it["abstract3 tests nothing", "example_in_abstract_class_that_directly_inherits_from_concrete_class"] = () => LogExample(classId: "3");
            }

            void before_each()
            {
                LogBefore(classId: "3");
            }

            void act_each()
            {
                LogAct(classId: "3");
            }

            void after_each()
            {
                LogAfter(classId: "3");
            }
        }

        abstract class Class4 : Class3
        {
            void abstract4_example()
            {
                it["abstract4 tests nothing", "example_in_abstract_class_that_inherits_another_abstract_class"] = () => LogExample(classId: "4");
            }

            void before_each()
            {
                LogBefore(classId: "4");
            }

            void act_each()
            {
                LogAct(classId: "4");
            }

            void after_each()
            {
                LogAfter(classId: "4");
            }
        }

        class Class5 : Class4
        {
            void concrete5_example()
            {
                it["concrete5 tests nothing", "example_in_concrete_class_that_inherits_an_abstract_class_with_deep_inheritance_chain"] = () => LogExample(classId: "5");
            }

            void before_each()
            {
                LogBefore(classId: "5");
            }

            void act_each()
            {
                LogAct(classId: "5");
            }

            void after_each()
            {
                LogAfter(classId: "5");
            }
        }

        [Test(Description = "before_each() in concrete classes affects base abstracts"),
         TestCase(typeof(Class2), "example_in_abtract_class", "12"),
         TestCase(typeof(Class2), "example_in_concrete_class_that_inherits_abstract", "12"),
         TestCase(typeof(Class5), "example_in_abstract_class_that_directly_inherits_from_concrete_class", "12345"),
         TestCase(typeof(Class5), "example_in_abstract_class_that_inherits_another_abstract_class", "12345"),
         TestCase(typeof(Class5), "example_in_concrete_class_that_inherits_an_abstract_class_with_deep_inheritance_chain", "12345")]
        public void before_eaches_should_run_in_the_correct_order(Type withRespectToContext, string tags, string beforeExecutionLog)
        {
            Init(withRespectToContext, tags).Run();

            var specInstance = classContext.GetInstance() as Class1;

            specInstance.beforeExecutionOrder.should_be(beforeExecutionLog);
        }

        [Test(Description = "act_each() in concrete classes affects base abstracts"),
         TestCase(typeof(Class2), "example_in_abtract_class", "12"),
         TestCase(typeof(Class2), "example_in_concrete_class_that_inherits_abstract", "12"),
         TestCase(typeof(Class5), "example_in_abstract_class_that_directly_inherits_from_concrete_class", "12345"),
         TestCase(typeof(Class5), "example_in_abstract_class_that_inherits_another_abstract_class", "12345"),
         TestCase(typeof(Class5), "example_in_concrete_class_that_inherits_an_abstract_class_with_deep_inheritance_chain", "12345")]
        public void act_eaches_should_run_in_the_correct_order(Type withRespectToContext, string tags, string actExecutionLog)
        {
            Init(withRespectToContext, tags).Run();

            var specInstance = classContext.GetInstance() as Class1;

            specInstance.actExecutionOrder.should_be(actExecutionLog);
        }

        [Test(Description = "after_each() in concrete classes affects base abstracts"),
         TestCase(typeof(Class2), "example_in_abtract_class", "21"),
         TestCase(typeof(Class2), "example_in_concrete_class_that_inherits_abstract", "21"),
         TestCase(typeof(Class5), "example_in_abstract_class_that_directly_inherits_from_concrete_class", "54321"),
         TestCase(typeof(Class5), "example_in_abstract_class_that_inherits_another_abstract_class", "54321"),
         TestCase(typeof(Class5), "example_in_concrete_class_that_inherits_an_abstract_class_with_deep_inheritance_chain", "54321")]
        public void after_eaches_should_run_in_the_correct_order(Type withRespectToContext, string tags, string afterExecutionLog)
        {
            Init(withRespectToContext, tags).Run();

            var specInstance = classContext.GetInstance() as Class1;

            specInstance.afterExecutionOrder.should_be(afterExecutionLog);
        }

        [Test,
         TestCase(typeof(Class2), "example_in_abtract_class", "b1b2ac1ac2i1af2af1"),
         TestCase(typeof(Class2), "example_in_concrete_class_that_inherits_abstract", "b1b2ac1ac2i2af2af1"),
         TestCase(typeof(Class5), "example_in_abstract_class_that_directly_inherits_from_concrete_class", "b1b2b3b4b5ac1ac2ac3ac4ac5i3af5af4af3af2af1"),
         TestCase(typeof(Class5), "example_in_abstract_class_that_inherits_another_abstract_class", "b1b2b3b4b5ac1ac2ac3ac4ac5i4af5af4af3af2af1"),
         TestCase(typeof(Class5), "example_in_concrete_class_that_inherits_an_abstract_class_with_deep_inheritance_chain", "b1b2b3b4b5ac1ac2ac3ac4ac5i5af5af4af3af2af1")]
        public void execution_should_run_in_the_correct_order(Type withRespectToContext, string tag, string fullExecutionLog)
        {
            Init(withRespectToContext, tag).Run();

            var specInstance = classContext.GetInstance() as Class1;

            specInstance.allExecutions.should_be(fullExecutionLog);
        }
    }
}
