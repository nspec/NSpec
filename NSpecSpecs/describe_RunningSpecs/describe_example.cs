using System;
using NSpec;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category( "RunningSpecs" )]
    public class describe_example : when_running_specs
    {
        class SpecClass : nspec
        {
            void it_changes_status_after_run()
            {
            }

            void it_passes()
            {
            }

            void it_fails()
            {
                throw new Exception();
            }
        }

        [Test]
        public void execution_status_changes_after_run()
        {
            Build( typeof( SpecClass ) );

            var ex = TheExample( "it changes status after run" );

            ex.HasRun.should_be_false();

            Run();

            ex.HasRun.should_be_true();
        }

        [Test]
        public void passing_status_is_passed_when_it_succeeds()
        {
            Run( typeof( SpecClass ) );

            TheExample( "it passes" ).Passed.should_be_true();
        }

        [Test]
        public void passing_status_is_not_passed_when_it_fails()
        {
            Run( typeof( SpecClass ) );

            TheExample( "it fails" ).Passed.should_be_false();
        }
    }
}