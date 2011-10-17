using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category( "RunningSpecs" )]
    public class describe_context_level_tagging : when_running_specs
    {
        class SpecClass : nspec
        {
            void has_tags_in_contexts()
            {
                context[ "is tagged with '@mytag'", "@mytag" ] = () =>
                {
                    it[ "is tagged with '@mytag'" ] = () => { 1.should_be( 1 ); };
                };

                context[ "has three tags", "mytag, expect-to-failure, foobar" ] = () =>
                {
                    it[ "has three tags" ] = () => { 1.should_be( 1 ); };
                };

                context[ "does not have a tag" ] = () =>
                {
                    it[ "does not have a tag" ] = () => { true.should_be_true(); };
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            Run( typeof( SpecClass ) );
        }

        [Test]
        public void does_not_have_a_tag()
        {
            TheContext( "does not have a tag" ).Tags.Count.should_be( 0 );
        }

        [Test]
        public void is_tagged_with_at_mytag()
        {
            TheContext( "is tagged with '@mytag'" ).Tags.should_contain_tag( "@mytag" );
            TheContext( "is tagged with '@mytag'" ).Tags.should_contain_tag( "mytag" );
        }

        [Test]
        public void has_three_tags()
        {
            TheContext( "has three tags" ).Tags.Count.should_be( 3 );
            TheContext( "has three tags" ).Tags.should_contain_tag( "mytag" );
            TheContext( "has three tags" ).Tags.should_contain_tag( "expect-to-failure" );
            TheContext( "has three tags" ).Tags.should_contain_tag( "@foobar" );
        }

        Context TheContext( string name )
        {
            return classContext.Contexts[0].Contexts.Single( context => context.Name == name );
        }
    }
}