using System;
using System.Linq;
using NUnit.Framework;
using NSpec;
using NSpec.Domain;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_before_contains_exception : when_running_specs
    {
        class SpecClass : nspec
        {
            void method_level_context()
            {
                before = () => { throw new InvalidOperationException(); };

                it["should fail this example because of before"] = () => "1".should_be("1");

                it["should also fail this example because of before"] = () => "1".should_be("1");

                context[ "exception thrown by both before and act" ] = () =>
                {
                    act = () => { throw new ArgumentException( "this exception should never be thrown" ); };

                    it[ "tracks only the first exception from 'before'" ] = () => "1".should_be( "1" );
                };
			}
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void it_should_fail_all_examples_in_before()
        {
            TheExample("should fail this example because of before").Exception.GetType().should_be(typeof(InvalidOperationException));
            TheExample("should also fail this example because of before").Exception.GetType().should_be(typeof(InvalidOperationException));
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_act()
        {
            TheExample( "tracks only the first exception from 'before'" ).Exception.GetType().should_be( typeof( InvalidOperationException ) );
        }

        private Example TheExample(string name)
        {
            return classContext.Contexts.First().AllExamples().Single(s => s.Spec == name);
        }
    }
}
