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
            conventions = new DefaultConventions();

            conventions.Initialize();

            parentContext = new ClassContext(typeof(parent_act), conventions);

            childContext = new ClassContext(typeof(child_act), conventions);

            parentContext.AddContext(childContext);

            parentContext.Build();

            childContext.Build();

            instance = new child_act();

            parentContext.SetInstanceContext(instance);

            childContext.SetInstanceContext(instance);
        }

        [Test]
        public void should_set_the_proper_before()
        {
            childContext.Act();

            instance.actResult.should_be("child");
        }

        [Test]
        public void it_should_also_set_the_proper_before_on_ancestors()
        {
            parentContext.Act();

            instance.actResult.should_be("parent");
        }

        private ClassContext childContext;

        private DefaultConventions conventions;

        private ClassContext parentContext;

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
            conventions = new DefaultConventions();

            conventions.Initialize();

            parentContext = new ClassContext(typeof(parent_before), conventions);

            childContext = new ClassContext(typeof(child_before), conventions);

            parentContext.AddContext(childContext);

            parentContext.Build();

            childContext.Build();
        }

        [Test]
        public void the_root_context_should_be_the_parent()
        {
            parentContext.Name.should_be(typeof(parent_before).Name.Replace("_", " "));
        }

        [Test]
        public void it_should_have_the_child_as_a_context()
        {
            parentContext.Contexts.First().Name.should_be(typeof(child_before).Name.Replace("_", " "));
        }

        private ClassContext childContext;

        private DefaultConventions conventions;

        private ClassContext parentContext;
    }

    [TestFixture]
    [Category("Context")]
    public class when_creating_before_contexts_for_derived_class
    {
        [SetUp]
        public void setup()
        {
            conventions = new DefaultConventions();

            conventions.Initialize();

            parentContext = new ClassContext(typeof(parent_before), conventions);

            childContext = new ClassContext(typeof(child_before), conventions);

            parentContext.AddContext(childContext);

            childContext.Build();

            parentContext.Build();

            instance = new child_before();

            childContext.SetInstanceContext(instance);

            parentContext.SetInstanceContext(instance);
        }
        
        [Test]
        public void should_set_the_proper_before()
        {
            childContext.Before();

            instance.beforeResult.should_be("child");
        }

        [Test]
        public void it_should_also_set_the_proper_before_on_ancestors()
        {
            parentContext.Before();

            instance.beforeResult.should_be("parent");
        }

        private ClassContext childContext;

        private DefaultConventions conventions;

        private ClassContext parentContext;

        private child_before instance;
    }
}