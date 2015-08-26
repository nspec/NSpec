using System.Collections.Generic;
using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.describe_before_and_after
{
    [TestFixture]
    public class improperly_formed_context_methods : when_running_specs
    {
        class before_all_sampleSpec : nspec
        {
            public static List<string> sequence = new List<string>();

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
                //notice, it would sort alphabetically before the before_all method
                sequence.Add("another_messed_up_context");
            }
        }

        [Test]
        public void it_runs_things_in_a_strange_order()
        {
            before_all_sampleSpec.sequence = new List<string>();

            Run(typeof (before_all_sampleSpec));

            //the two improperly crafted context methods are executed first in the order they were declared
            //while nspec is building up its model of contexts and examples
            //then the class level before and properly crafted spec (wrapped in lambda) is executed.

            //The moral of the story is context methods that don't have their behavior wrapped 
            //in lambdas (incorrectly so), run in the order that they are declared (disregarding alphabetical ordering).
            //FYI, alphabetical ordering can easily be implemented in the 'Methods' extension method.
            CollectionAssert.AreEqual(new[] { "messed_up_context", "another_messed_up_context", "before_all", "a_regular_context_method" }, before_all_sampleSpec.sequence);
        }

    }
}