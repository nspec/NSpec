using NSpec;
using NUnit.Framework;

namespace NSpecSpecs.WhenRunningSpecs
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
                    it["is tagged with 'mytag'"] = () => { 1.should_be(1); };
                };

                context["has three tags", "mytag, expect-to-failure, foobar"] = () =>
                {
                    it["has three tags"] = () => { 1.should_be(1); };
                };

                context["does not have a tag"] = () =>
                {
                    it["does not have a tag"] = () => { true.should_be_true(); };
                };

                context["has a nested context", "nested-tag"] = () =>
                {
                    context["is the nested context"] = () =>
                    {
                        it["is the nested example"] = () => { true.should_be_true(); };
                    };
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            Init(typeof(SpecClass)).Run();
        }

        [Test]
        public void it_only_contains_default_tag()
        {
            TheContext("does not have a tag").Tags.should_contain("SpecClass");
        }

        [Test]
        public void is_tagged_with_mytag()
        {
            TheContext("is tagged with 'mytag'").Tags.should_contain_tag("mytag");
        }

        [Test]
        public void has_three_tags_and_the_default()
        {
            TheContext("has three tags").Tags.should_contain_tag("SpecClass");
            TheContext("has three tags").Tags.should_contain_tag("mytag");
            TheContext("has three tags").Tags.should_contain_tag("expect-to-failure");
            TheContext("has three tags").Tags.should_contain_tag("foobar");
            TheContext("has three tags").Tags.Count.should_be(4);
        }

        [Test]
        public void nested_contexts_should_inherit_the_tag()
        {
            TheContext("has a nested context").Tags.should_contain_tag("nested-tag");
            TheContext("is the nested context").Tags.should_contain_tag("nested-tag");
        }

        [Test]
        public void nested_examples_should_inherit_the_tag()
        {
            TheContext("has a nested context").Tags.should_contain_tag("nested-tag");
            TheExample("is the nested example").Tags.should_contain_tag("nested-tag");
        }
    }
}