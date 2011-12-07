using System.Collections.Generic;
using System.Reflection;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs
{
    [TestFixture]
    public class describe_Method_Invocation_Sequence : when_running_specs
    {
        class before_all_sampleSpec : nspec
        {
            public List<string> sequence = new List<string>();

            //these methods are purposely declared in a strange order. Read on for more detail

            void a_regular_context_method()
            {
                //this context method wraps everything in lambdas (as context methods should),
                //because it's behavior is deferred using lambdas, it is run after the methods declared below.
                it["uses lambdas"] = () => sequence.Add("a_regular_context_method");
            }

            void messed_up_context()
            {
                //this contrived context method really should have everything wrapped in lambdas
                sequence.Add("messed_up_context");
            }

            void before_all()
            {
                //this is a properly crafted class level method, without lambda
                sequence.Add("before_all");
            }

            void another_messed_up_context()
            {
                //this context method should also have everything wrapped in lambdas
                //notice, it would sort alphebectically before the before_all method
                sequence.Add("another_messed_up_context");
            }

            //The moral of the story is context methods that don't have their behavior wrapped 
            //in lambdas (incorrectly so), run in the order that they are declared (disregarding alphabetical ordering).
            //Serindipitously, this means before_all's declared at the top, run as expected: before everything else.
            //Since we only create instances once, it also means they only run once.
        }

        [Test]
        public void it_runs_things_in_a_strange_order()
        {
            var invocation = new RunnerInvocation(Assembly.GetExecutingAssembly().Location,
                                    typeof(before_all_sampleSpec).Name,
                                    new SilentLiveFormatter());

            contexts = invocation.Runner().Run();

            var instance = contexts.Find("before all sampleSpec").GetInstance() as before_all_sampleSpec;

            CollectionAssert.AreEqual(new[] { "messed_up_context", "before_all", "another_messed_up_context", "a_regular_context_method" }, instance.sequence);
        }

    }
}