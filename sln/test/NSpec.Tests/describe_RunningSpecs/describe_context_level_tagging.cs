using FluentAssertions;
using NUnit.Framework;

namespace NSpec.Tests.WhenRunningSpecs
{
    [TestFixture]
    public class describe_context_level_tagging : when_running_specs
    {
        class SpecClass : nspec
        {
            void has_tags_in_contexts()
            {
                context["is tagged with 'mytag'", "mytag"] = () =>
                {
                    it["is tagged with 'mytag'"] = () => { Assert.That(true, Is.True); };
                };

                context["has three tags", "mytag, expect-to-failure, foobar"] = () =>
                {
                    it["has three tags"] = () => { Assert.That(true, Is.True); };
                };

                context["does not have a tag"] = () =>
                {
                    it["does not have a tag"] = () => { Assert.That(true, Is.True); };
                };

                context["has a nested context", "nested-tag"] = () =>
                {
                    context["is the nested context"] = () =>
                    {
                        it["is the nested example"] = () => { Assert.That(true, Is.True); };
                    };
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            Run(typeof(SpecClass));
        }

        [Test]
        public void it_only_contains_default_tag()
        {
            TheContext("does not have a tag").Tags.Should().Contain("SpecClass");
        }

        [Test]
        public void is_tagged_with_mytag()
        {
            TheContext("is tagged with 'mytag'").Tags.Should().Contain("mytag");
        }

        [Test]
        public void has_three_tags_and_the_default()
        {
            TheContext("has three tags").Tags.Should().Contain("SpecClass");
            TheContext("has three tags").Tags.Should().Contain("mytag");
            TheContext("has three tags").Tags.Should().Contain("expect-to-failure");
            TheContext("has three tags").Tags.Should().Contain("foobar");
            TheContext("has three tags").Tags.Count.Should().Be(4);
        }

        [Test]
        public void nested_contexts_should_inherit_the_tag()
        {
            TheContext("has a nested context").Tags.Should().Contain("nested-tag");
            TheContext("is the nested context").Tags.Should().Contain("nested-tag");
        }

        [Test]
        public void nested_examples_should_inherit_the_tag()
        {
            TheContext("has a nested context").Tags.Should().Contain("nested-tag");
            TheExample("is the nested example").Tags.Should().Contain("nested-tag");
        }
    }
}