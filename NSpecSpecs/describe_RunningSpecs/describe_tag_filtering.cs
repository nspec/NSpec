using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category( "RunningSpecs" )]
    public class describe_tag_filtering : when_running_specs
    {
        class SpecClass : nspec
        {
            void has_tags_in_context_or_example_level()
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

                context[ "has a nested context" ] = () =>
                {
                    context[ "is the nested context", "@foobar" ] = () =>
                    {
                        it[ "is the nested example", "@nested-tag" ] = () => { true.should_be_true(); };
                    };
                };
            }
        }

        [Test]
        public void filters_include_tag()
        {
            Run( typeof( SpecClass ), new Tags().ParseTagFilters( "@mytag" ) );
            classContext.AllContexts().Count().should_be( 4 ); // class context + method context + 2 tagged sub-contexts
        }

        [Test]
        public void filters_exclude_tag()
        {
            Run( typeof( SpecClass ), new Tags().ParseTagFilters( "~@mytag" ) );
            classContext.AllContexts().Count().should_be( 5 );
            classContext.AllContexts().should_not_contain( c => c.Tags.Contains( "mytag" ) );
        }

        [Test]
        public void filters_include_and_exclude_tags()
        {
            Run( typeof( SpecClass ), new Tags().ParseTagFilters( "@mytag,~@foobar" ) );
            classContext.AllContexts().should_contain( c => c.Tags.Contains( "mytag" ) );
            classContext.AllContexts().should_not_contain( c => c.Tags.Contains( "foobar" ) );
            classContext.AllContexts().Count().should_be( 3 );
        }
    }
}