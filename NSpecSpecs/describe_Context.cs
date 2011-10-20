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

            child.AddExample(new Example("") { ExampleLevelException = new Exception() });

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
            parentContext = new ClassContext(typeof(parent_act));

            childContext = new ClassContext(typeof(child_act));

            parentContext.AddContext(childContext);

            instance = new child_act();

            parentContext.Build();
        }

        [Test]
        public void should_run_the_acts_in_the_right_order()
        {
            childContext.RunActs(instance);

            instance.actResult.should_be("parentchild");
        }

        ClassContext childContext, parentContext;

        child_act instance;
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
            parentContext = new ClassContext(typeof(parent_before));

            childContext = new ClassContext(typeof(child_before));

            parentContext.AddContext(childContext);

            instance = new child_before();

            parentContext.Build();
        }
        
        [Test]
        public void should_run_the_befores_in_the_proper_order()
        {
            childContext.RunBefores(instance);

            instance.beforeResult.should_be("parentchild");
        }

        ClassContext childContext, parentContext;

        child_before instance;
    }

    [TestFixture]
    [Category( "Context" )]
    public class when_trimming_empty_contexts
    {
        [Test]
        public void given_nested_contexts_with_and_without_examples()
        {
            var root = new Context( "root context" );

            // add context with NO example
            root.AddContext( new Context( "context with no example" ) );

            // add context with ONE example
            var context = new Context( "context with example" );
            context.AddExample( new Example( "example" ) );
            root.AddContext( context );

            // validate precondition
            root.Contexts.Count().should_be( 2 );

            // perform trim operation
            root.TrimEmptyDescendants();

            // validate postcondition
            root.Contexts.Count().should_be( 1 );
        }
    }
}