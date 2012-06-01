using System;
using System.Collections.Generic;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    class describe_context_level_afterAll : when_running_specs
    {
        class SpecClass : nspec
        {
            public static List<string> log;
            
            static SpecClass()
            {
                log = new List<string>();
            }

            void after_all()
            {
                log.Add("METHOD LEVEL AFTER ALL");
            }

            void after_each()
            {
                log.Add("method level after each");
            }

            void execution_of_context()
            {
                afterAll = () => log.Add("CONTEXT LEVEL AFTER ALL");

                afterEach = () => log.Add("context level after each");
                it["has test one"] = () => log.Add("test one executed");

                it["has test two"] = () => log.Add("test two executed");

                context["execution of tests three and four"] = () =>
                {
                    afterAll = () => log.Add("CONTEXT 2 LEVEL AFTER ALL");
                    afterEach = () => log.Add("context 2 level after each");

                    it["has test three"] = () => log.Add("test three executed");

                    it["has test four"] = () => log.Add("test four executed");
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof (SpecClass));
        }

        [Test]
        public void should_execute_after_all_only_for_each_context_level()
        {
            SpecClass.log.should_be(new[]
                {
                    "test one executed",
                    "context level after each",
                    "CONTEXT LEVEL AFTER ALL",
                    "method level after each",
                    "METHOD LEVEL AFTER ALL",
                    "test two executed",
                    "context level after each",
                    "CONTEXT LEVEL AFTER ALL",
                    "method level after each",
                    "METHOD LEVEL AFTER ALL",
                    "test three executed",
                    "context 2 level after each",
                    "CONTEXT 2 LEVEL AFTER ALL",
                    "context level after each",
                    "CONTEXT LEVEL AFTER ALL",
                    "method level after each",
                    "METHOD LEVEL AFTER ALL",
                    "test four executed",
                    "context 2 level after each",
                    "CONTEXT 2 LEVEL AFTER ALL",
                    "context level after each",
                    "CONTEXT LEVEL AFTER ALL",
                    "method level after each",
                    "METHOD LEVEL AFTER ALL"
                });
        }
    }
}
