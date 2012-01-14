using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpecSpecs.WhenRunningSpecs;
using NSpec;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    class describe_context_level_beforeAll : when_running_specs
    {
        class SpecClass : nspec
        {
            public static List<string> log = null;

            static SpecClass()
            {
                log = new List<string>();	
            }

            void before_all()
            {
                log.Clear();

                log.Add("METHOD LEVEL BEFORE ALL");
            }

            void before_each()
            {
                log.Add("method level before each");
            }

            void execution_of_context()
            {
                beforeAll = () => log.Add("CONTEXT LEVEL BEFORE ALL");

                beforeEach = () => log.Add("context level before each");

                it["has test one"] = () => log.Add("test one executed");

                it["has test two"] = () => log.Add("test two executed");

                context["execution of additional context"] = () =>
                {
                    beforeAll = () => log.Add("SUB CONTEXT LEVEL BEFORE ALL");

                    beforeEach = () => log.Add("sub context level before each");

                    it["has test three"] = () => log.Add("test three executed");

                    it["has test four"] = () => log.Add("test four executed");
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
        public void should_execute_before_all_only_for_each_context_level()
        {
            SpecClass.log.should_be(new string[] 
            { 
                "METHOD LEVEL BEFORE ALL",
                "method level before each",
                "CONTEXT LEVEL BEFORE ALL",
                "context level before each",
                "test one executed",
                "method level before each",
                "context level before each",
                "test two executed",
                "method level before each",
                "context level before each",
                "SUB CONTEXT LEVEL BEFORE ALL",
                "sub context level before each",
                "test three executed",
                "method level before each",
                "context level before each",
                "sub context level before each",
                "test four executed",
            });
        }
    }
}
