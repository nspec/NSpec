using NSpec;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;

namespace NSpecSpecs.describe_RunningSpecs.after_all
{
    class SequenceSpec : nspec { public static string sequence; }

    class SpecClass : SequenceSpec
    {
        void as_long_as_the_world_has_not_come_to_an_end()
        {
            beforeAll = () => sequence = "A";
            before = () => sequence += "B";
            specify = () => 1.Is(1);
            specify = () => 2.Is(2); //two specs cause before_each and after_each to run twice
            afterEach = () => sequence += "C";
            afterAll = () => sequence += "D";
        }
    }

    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_after_all : when_running_specs
    {
        [Test]
        public void everything_runs_in_the_correct_order_and_with_the_correct_frequency()
        {
            Run(typeof(SpecClass));

            SpecClass.sequence.Is("ABCBCD");
            //SpecClass.log.should_be(new[]
            //    {
            //        "context_b beforeEach",
            //        "context_b it 1",
            //        "context_b afterEach",
            //        "DerivedClass after_each",
            //        "SpecClass after_each",
            //        "context_b beforeEach",
            //        "context_b it 2",
            //        "context_b afterEach",
            //        "DerivedClass after_each",
            //        "SpecClass after_each",
            //        "context_b afterAll",
            //        "DerivedClass after_all",
            //        "SpecClass after_all"
            //    });
        }

    }
}
