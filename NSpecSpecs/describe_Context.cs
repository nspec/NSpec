using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using System.Linq;
using System;

namespace NSpecNUnit
{
    [TestFixture]
    [Category("Context")]
    public class describe_Context
    {
        [Test]
        public void it_should_make_a_sentence_from_underscored_context_names()
        {
            new Context("i_love_underscores").Name.should_be("i love underscores");
        }
    }

    [TestFixture]
    [Category("Context")]
    public class when_counting_failures
    {
        [Test]
        public void given_nested_contexts_and_the_child_has_a_failure()
        {
            var child = new Context("child");

            child.AddExample(new Example("") { Exception = new Exception() });

            var parent = new Context("parent");

            parent.AddContext(child);

            parent.Failures().Count().should_be(1);
        }
    }

    public class child_act : parent_act
    {
        void act_each()
        {
            actResult += "child";
        }
    }

    public class parent_act : nspec
    {
        public string actResult;
        void act_each()
        {
            actResult = "parent";
        }
    }

    [TestFixture]
    [Category("Context")]
    public class when_creating_act_contexts_for_derived_class
    {
        [SetUp]
        public void setup()
        {
            context = typeof(child_act).RootContext();

            instance = new child_act();

            context.Contexts.First().SetInstanceContext(instance);
        }

        [Test]
        public void should_set_the_proper_before()
        {
            context.Contexts.First().Act();

            instance.actResult.should_be("child");
        }

        [Test]
        public void it_should_also_set_the_proper_before_on_ancestors()
        {
            context.Act();

            instance.actResult.should_be("parent");
        }

        private Context context;
        private child_act instance;
    }

    public class child_before : parent_before
    {
        void before_each()
        {
            beforeResult += "child";
        }
    }

    public class parent_before : nspec
    {
        public string beforeResult;
        void before_each()
        {
            beforeResult = "parent";
        }
    }

    [TestFixture]
    [Category("Context")]
    public class when_creating_contexts_for_derived_classes
    {
        [SetUp]
        public void setup()
        {
            context = typeof(child_before).RootContext();
        }

        [Test]
        public void the_root_context_should_be_the_be_the_parent()
        {
            context.Name.should_be(typeof(parent_before).Name.Replace("_", " "));
        }

        [Test]
        public void it_should_have_the_child_as_a_context()
        {
            context.Contexts.First().Name.should_be(typeof(child_before).Name.Replace("_", " "));
        }

        private Context context;
    }

    [TestFixture]
    [Category("Context")]
    public class when_creating_before_contexts_for_derived_class
    {
        [SetUp]
        public void setup()
        {
            context = typeof(child_before).RootContext();

            instance = new child_before();

            context.Contexts.First().SetInstanceContext(instance);
        }

        [Test]
        public void should_set_the_proper_before()
        {
            context.Contexts.First().Before();

            instance.beforeResult.should_be("child");
        }

        [Test]
        public void it_should_also_set_the_proper_before_on_ancestors()
        {
            context.Before();

            instance.beforeResult.should_be("parent");
        }

        private Context context;
        private child_before instance;
    }
}