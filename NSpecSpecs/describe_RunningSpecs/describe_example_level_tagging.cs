using System.Collections.Generic;
using System.Linq;
using NSpec;
using NSpec.Domain;
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
                it["is tagged with 'mytag'", "mytag"] = () => { 1.should_be(1); };

                it["has three tags", "mytag, expect-to-failure, foobar"] = () => { 1.should_be(1); };

                it["does not have a tag"] = () => { true.should_be_true(); };
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
            TheExample("does not have a tag").Tags.should_contain("SpecClass");
        }

        [Test]
        public void is_tagged_with_at_mytag()
        {
            TheExample("is tagged with 'mytag'").Tags.should_contain_tag("mytag");
        }

        [Test]
        public void has_three_tags_and_default_class_tag()
        {
            TheExample("has three tags").Tags.should_contain_tag("SpecClass");
            TheExample("has three tags").Tags.should_contain_tag("mytag");
            TheExample("has three tags").Tags.should_contain_tag("expect-to-failure");
            TheExample("has three tags").Tags.should_contain_tag("foobar");
            TheExample("has three tags").Tags.Count.should_be(4);
        }
    }
}