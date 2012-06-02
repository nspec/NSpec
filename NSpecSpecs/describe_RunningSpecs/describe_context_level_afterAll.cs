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

            void after_each()
            {
                log.Add("method level after each");
            }

            void after_All()
            {
                log.Add("METHOD LEVEL AFTER ALL");
            }

            void execution_of_context()
            {
                afterAll = () => log.Add("CONTEXT LEVEL AFTER ALL");

                afterEach = () => log.Add("context level after each");
                it["has test one"] = () => log.Add("test one executed");

                it["has test two"] = () => log.Add("test two executed");
            }

            void execution_of_context_2()
            {
                afterAll = () => log.Add("CONTEXT 2 LEVEL AFTER ALL");

                it["has test three"] = () => log.Add("test three executed");

                it["has test four"] = () => log.Add("test four executed");

                context["execution of tests five and six"] = () =>
                {
                    afterAll = () => log.Add("CONTEXT 3 LEVEL AFTER ALL");
                    afterEach = () => log.Add("context 3 level after each");

                    it["has test five"] = () => log.Add("test five executed");

                    it["has test six"] = () => log.Add("test six executed");
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
                    "method level after each",
                    "test two executed",
                    "context level after each",
                    "method level after each",
                    "CONTEXT LEVEL AFTER ALL",
                    "test three executed",
                    "method level after each",
                    "test four executed",
                    "method level after each",
                    "test five executed",
                    "context 3 level after each",
                    "method level after each",
                    "test six executed",
                    "context 3 level after each",
                    "method level after each",
                    "CONTEXT 3 LEVEL AFTER ALL",
                    "CONTEXT 2 LEVEL AFTER ALL",
                    "METHOD LEVEL AFTER ALL"
                });
        }
    }
}
