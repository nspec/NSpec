using System;
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
        [Tag( "@class-tag-zero" )]
        class SpecClass0 : nspec
        {
            void it_has_an_empty_example()
            {
            }
        }

        [Tag( "@class-tag" )]
        class SpecClass : nspec
        {
            [Tag( "@method-tag-one" )]
            void has_tag_at_method_level_context()
            {
                it[ "tests nothing" ] = () => 1.should_be( 1 );
            }

            [Tag( "@method-tag-two" )]
            void has_tags_in_context_or_example_level()
            {
                context[ "is tagged with '@mytag'", "@mytag" ] = () =>
                {
                    it[ "is tagged with '@mytag'" ] = () => 1.should_be( 1 );
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
        public void includes_tag()
        {
            Run( typeof( SpecClass ), new Tags().ParseTagFilters( "@mytag" ) );
            classContext.AllContexts().Count().should_be( 4 ); // class context + method context + 2 tagged sub-contexts
        }

        [Test]
        public void excludes_tag()
        {
            Run( typeof( SpecClass ), new Tags().ParseTagFilters( "~@mytag" ) );
            classContext.AllContexts().Count().should_be( 6 );
            classContext.AllContexts().should_not_contain( c => c.Tags.Contains( "mytag" ) );
        }

        [Test]
        public void includes_and_excludes_tags()
        {
            Run( typeof( SpecClass ), new Tags().ParseTagFilters( "@mytag,~@foobar" ) );
            classContext.AllContexts().should_contain( c => c.Tags.Contains( "mytag" ) );
            classContext.AllContexts().should_not_contain( c => c.Tags.Contains( "foobar" ) );
            classContext.AllContexts().Count().should_be( 3 );
        }

        [Test]
        public void includes_tag_as_class_attribute()
        {
            Run( typeof( SpecClass0 ), new Tags().ParseTagFilters( "@class-tag-zero" ) );
            classContext.AllContexts().Count().should_be( 1 );
        }

        [Test]
        public void excludes_tag_as_class_attribute()
        {
            Run( new[] { typeof( SpecClass ), typeof( SpecClass0 ) }, new Tags().ParseTagFilters( "~@class-tag" ) );
            contextCollection.Count.should_be( 1 );
        }

        [Test]
        public void includes_tag_as_method_attribute()
        {
            Run( typeof( SpecClass ), new Tags().ParseTagFilters( "@method-tag-one" ) );
            classContext.AllContexts().Count().should_be( 2 );
        }

        [Test]
        public void excludes_tag_as_method_attribute()
        {
            Run( typeof( SpecClass ), new Tags().ParseTagFilters( "~@method-tag-one" ) );
            classContext.AllContexts().Count().should_be( 7 );
        }
    }
}