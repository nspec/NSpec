using NSpec.Domain;
using NUnit.Framework;
using FluentAssertions;

namespace NSpec.Tests
{
    [TestFixture]
    public class describe_parsing_tags
    {
        [Test]
        public void parses_single_tag()
        {
            var tags = Tags.ParseTags("mytag");
            tags.Should().Contain("mytag");
        }

        [Test]
        public void parses_multiple_tags()
        {
            var tags = Tags.ParseTags("myTag1,myTag2");
            tags.Should().Contain("myTag1");
            tags.Should().Contain("myTag2");
            tags.Count.Should().Be(2);
        }

        [Test]
        public void parses_single_include_tag_filters()
        {
            var tags = new Tags();
            tags.Parse("mytag");
            tags.IncludeTags.Should().Contain("mytag");
        }

        [Test]
        public void parses_single_exclude_tag_filters()
        {
            var tags = new Tags();
            tags.Parse("~mytag");
            tags.ExcludeTags.Should().Contain("mytag");
        }

        [Test]
        public void parses_multiple_tags_filters()
        {
            var tags = new Tags();
            tags.Parse("myInclude1,~myExclude1,myInclude2,~myExclude2,");

            tags.IncludeTags.Should().Contain("myInclude1");
            tags.ExcludeTags.Should().Contain("myExclude1");

            tags.IncludeTags.Should().Contain("myInclude2");
            tags.ExcludeTags.Should().Contain("myExclude2");

            tags.IncludeTags.Count.Should().Be(2);
            tags.ExcludeTags.Count.Should().Be(2);
        }
    }
}
