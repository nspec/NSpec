using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_parsing_tags : when_running_specs
    {
        [Test]
        public void parses_single_tag()
        {
            var tags = Tags.ParseTags("mytag");
            tags.should_contain_tag("mytag");
        }

        [Test]
        public void parses_ampersat_tag()
        {
            var tags = Tags.ParseTags("@mytag");
            tags.should_contain_tag("mytag");
        }

        [Test]
        public void parses_multiple_tags()
        {
            var tags = Tags.ParseTags("myTag1,@myTag2");
            tags.should_contain_tag("@myTag1");
            tags.should_contain_tag("myTag2");
            tags.Count.should_be(2);
        }

        [Test]
        public void parses_single_include_tag_filters()
        {
            var tags = new Tags();
            tags.Parse("mytag");
            tags.should_tag_as_included("mytag");
        }

        [Test]
        public void parses_single_exclude_tag_filters()
        {
            var tags = new Tags();
            tags.Parse("~mytag");
            tags.should_tag_as_excluded("mytag");
        }

        [Test]
        public void parses_ampersat_tag_filters()
        {
            var tags = new Tags();
            tags.Parse("@mytag");
            tags.should_tag_as_included("mytag");
        }

        [Test]
        public void parses_multiple_tags_filters()
        {
            var tags = new Tags();
            tags.Parse("myInclude1,~myExclude1,@myInclude2,~@myExclude2,");
            tags.should_tag_as_excluded("@myExclude1");
            tags.should_tag_as_excluded("myExclude2");
            tags.should_tag_as_included("@myInclude1");
            tags.should_tag_as_included("myInclude2");
            tags.IncludeTags.Count.should_be(2);
            tags.ExcludeTags.Count.should_be(2);
        }
    }
}