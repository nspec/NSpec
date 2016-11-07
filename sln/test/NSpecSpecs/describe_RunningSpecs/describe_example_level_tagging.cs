using FluentAssertions;
using NSpec;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class describe_example_level_tagging : when_running_specs
    {
        class SpecClass : nspec
        {
            void has_tags_in_examples()
            {
                it["is tagged with 'mytag'", "mytag"] = () => { 1.Should().Be(1); };

                it["has three tags", "mytag, expect-to-failure, foobar"] = () => { 1.Should().Be(1); };

                it["does not have a tag"] = () => { true.Should().BeTrue(); };
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void it_only_has_the_default_class_tag()
        {
            TheExample("does not have a tag").Tags.Should().Contain("SpecClass");
        }

        [Test]
        public void is_tagged_with_at_mytag()
        {
            TheExample("is tagged with 'mytag'").Tags.Should().Contain("mytag");
        }

        [Test]
        public void has_three_tags_and_default_class_tag()
        {
            TheExample("has three tags").Tags.Should().Contain("SpecClass");
            TheExample("has three tags").Tags.Should().Contain("mytag");
            TheExample("has three tags").Tags.Should().Contain("expect-to-failure");
            TheExample("has three tags").Tags.Should().Contain("foobar");
            TheExample("has three tags").Tags.Count.Should().Be(4);
        }
    }
}