using System.Linq;
using NSpec;
using NUnit.Framework;
using FluentAssertions;

namespace NSpecSpecs.WhenRunningSpecs
{
    [TestFixture]
    public class describe_tag_filtering : when_running_specs
    {
        [Tag("class-tag-zero")]
        class SpecClass0 : nspec
        {
            [Tag("method-tag-zero")]
            void it_has_an_empty_example()
            {

            }
        }

        abstract class SpecClassBase : nspec
        {
            void specify_empty_example()
            {
                
            }
        }

        class SpecClassDerived : SpecClassBase
        {
            void specify_another_empty_example()
            {
                
            }
        }

        [Tag("class-tag")]
        class SpecClass : nspec
        {
            [Tag("method-tag-one")]
            void has_tag_at_method_level_context()
            {
                it["tests nothing"] = () => 1.Should().Be(1);
            }

            [Tag("method-tag-two")]
            void has_tags_in_context_or_example_level()
            {
                context["is tagged with 'mytag'", "mytag"] = () =>
                {
                    it["is tagged with 'mytag'"] = () => 1.Should().Be(1);
                };

                context["has three tags", "mytag,expect-to-failure,foobar"] = () =>
                {
                    it["has three tags"] = () => { 1.Should().Be(1); };
                };

                context["does not have a tag"] = () =>
                {
                    it["does not have a tag"] = () => { true.Should().BeTrue(); };
                };

                context["has a nested context"] = () =>
                {
                    context["is the nested context", "foobar"] = () =>
                    {
                        it["is the nested example", "nested-tag"] = () => { true.Should().BeTrue(); };
                    };
                };
            }
        }

        class SpecClass1 : nspec
        {
            void filters_out_not_run_examples()
            {
                context["has only example level tags"] = () =>
                {
                    it["should run and be in output", "shouldbeinoutput"] = () => true.Should().BeTrue();
                    it["should not run and not be in output", "barbaz"] = () => true.Should().BeTrue();
                    it["should also not run too not be in output"] = () => true.Should().BeTrue();

                    xit["pending but should be in output", "shouldbeinoutput"] = () => true.Should().BeTrue();
                    it["also pending but should be in output", "shouldbeinoutput"] = todo;
                };

                context["has context level tags", "shouldbeinoutput"] = () =>
                {
                    it["should also run and be in output", "barbaz"] = () => true.Should().BeTrue();
                    it["should yet also run and be in output"] = () => true.Should().BeTrue();
                };
            }
        }

        [Test]
        public void abstracted_classes_are_automatically_included_in_class_tags()
        {
            Run(typeof(SpecClassDerived));

            classContext.Tags.Should().Contain("SpecClassBase");

            classContext.Tags.Should().Contain("SpecClassDerived");
        }

        [Test]
        public void classes_are_automatically_tagged_with_class_name()
        {
            Run(typeof(SpecClass0));

            classContext.Tags.Should().Contain("class-tag-zero");

            classContext.Tags.Should().Contain("SpecClass0");
        }

        [Test]
        public void includes_tag()
        {
            tags = "mytag";
            Run(typeof(SpecClass));
            classContext.AllContexts().Count().Should().Be(4);
        }

        [Test]
        public void excludes_tag()
        {
            tags = "~mytag";
            Run(typeof(SpecClass));
            classContext.AllContexts().Count().Should().Be(6);
            classContext.AllContexts().Should().NotContain(c => c.Tags.Contains("mytag"));
        }

        [Test]
        public void includes_and_excludes_tags()
        {
            tags = "mytag,~foobar";
            Run(typeof(SpecClass));
            classContext.AllContexts().Should().Contain(c => c.Tags.Contains("mytag"));
            classContext.AllContexts().Should().NotContain(c => c.Tags.Contains("foobar"));
            classContext.AllContexts().Count().Should().Be(3);
        }

        [Test]
        public void includes_tag_as_class_attribute()
        {
            tags = "class-tag-zero";
            Run(typeof(SpecClass0));
            classContext.AllContexts().Count().Should().Be(1);
        }

        [Test]
        public void includes_tag_for_method_as_method_attribute()
        {
            tags = "method-tag-zero";
            Run(typeof(SpecClass0));
            classContext.AllContexts().SelectMany(s => s.Examples).Count().Should().Be(1);
        }

        [Test]
        public void excludes_tag_as_class_attribute()
        {
            tags = "~class-tag";
            Run(new[] { typeof(SpecClass), typeof(SpecClass0) });
            contextCollection.Count.Should().Be(1);
        }

        [Test]
        public void includes_tag_as_method_attribute()
        {
            tags = "method-tag-one";
            Run(typeof(SpecClass));
            classContext.AllContexts().Count().Should().Be(2);
        }

        [Test]
        public void excludes_tag_as_method_attribute()
        {
            tags = "~method-tag-one";
            Run(typeof(SpecClass));
            classContext.AllContexts().Count().Should().Be(7);
        }

        [Test]
        public void excludes_examples_not_run()
        {
            tags = "shouldbeinoutput";
            Run(typeof(SpecClass1));
            var allExamples = classContext.AllContexts().SelectMany(c => c.AllExamples()).ToList();
            allExamples.Should().Contain(e => e.Spec == "should run and be in output");
            allExamples.Should().Contain(e => e.Spec == "should also run and be in output");
            allExamples.Should().Contain(e => e.Spec == "should yet also run and be in output");
            allExamples.Should().Contain(e => e.Spec == "pending but should be in output");
            allExamples.Should().Contain(e => e.Spec == "also pending but should be in output");
            allExamples.Should().NotContain(e => e.Spec == "should not run and not be in output");
            allExamples.Should().NotContain(e => e.Spec == "should also not run too not be in output");
        }
    }
}
