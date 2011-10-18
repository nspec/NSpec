using System.Collections.Generic;
using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
	[TestFixture]
	[Category( "RunningSpecs" )]
	public class describe_tags : when_running_specs
	{
		class SpecClass : nspec
		{
			public void parses_tags()
			{
				it[ "parses single tag" ] = () =>
				{
					var tags = Tags.ParseTags( "mytag" );
					tags.should_contain_tag( "mytag" );
				};

				it[ "parses ampersat tag" ] = () =>
				{
					var tags = Tags.ParseTags( "@mytag" );
					tags.should_contain_tag( "mytag" );
				};

				it[ "parses multiple tags" ] = () =>
				{
					var tags = Tags.ParseTags( "myTag1,@myTag2" );
					tags.should_contain_tag( "@myTag1" );
					tags.should_contain_tag( "myTag2" );
					tags.Count.should_be( 2 );
				};
			}

			public void parses_tag_filters()
			{
				it[ "parses single 'include' tag filter" ] = () =>
				{
					var tags = new Tags();
					tags.ParseTagFilters( "mytag" );
					tags.should_tag_as_included( "mytag" );
				};

				it[ "parses single 'exclude' tag filter" ] = () =>
				{
					var tags = new Tags();
					tags.ParseTagFilters( "~mytag" );
					tags.should_tag_as_excluded( "mytag" );
				};

				it[ "parses ampersat tag filter" ] = () =>
				{
					var tags = new Tags();
					tags.ParseTagFilters( "@mytag" );
					tags.should_tag_as_included( "mytag" );
				};

				it[ "parses multiple tags filter" ] = () =>
				{
					var tags = new Tags();
					tags.ParseTagFilters( "myInclude1,~myExclude1,@myInclude2,~@myExclude2," );
					tags.should_tag_as_excluded( "@myExclude1" );
					tags.should_tag_as_excluded( "myExclude2" );
					tags.should_tag_as_included( "@myInclude1" );
					tags.should_tag_as_included( "myInclude2" );
					tags.IncludeTags.Count.should_be( 2 );
					tags.ExcludeTags.Count.should_be( 2 );
				};
			}
		}

		[SetUp]
		public void setup()
		{
			Run( typeof( SpecClass ) );
		}

		[Test]
		public void parses_single_tag()
		{
			TheExample( "parses single tag" ).ExampleLevelException.should_be( null );
		}

		[Test]
		public void parses_ampersat_tag()
		{
			TheExample( "parses ampersat tag" ).ExampleLevelException.should_be( null );
		}

		[Test]
		public void parses_multiple_tags()
		{
			TheExample( "parses multiple tags" ).ExampleLevelException.should_be( null );
		}

		[Test]
		public void parses_single_include_tag_filters()
		{
			TheExample( "parses single 'include' tag filter" ).ExampleLevelException.should_be( null );
		}

		[Test]
		public void parses_single_exclude_tag_filters()
		{
			TheExample( "parses single 'exclude' tag filter" ).ExampleLevelException.should_be( null );
		}

		[Test]
		public void parses_ampersat_tag_filters()
		{
			TheExample( "parses ampersat tag filter" ).ExampleLevelException.should_be( null );
		}

		[Test]
		public void parses_multiple_tags_filters()
		{
			TheExample( "parses multiple tags filter" ).ExampleLevelException.should_be( null );
		}

		private Example TheExample( string name )
		{
			return classContext.AllContexts().SelectMany( context => context.Examples.Where( example => example.Spec == name ) ).FirstOrDefault();
		}
	}
}