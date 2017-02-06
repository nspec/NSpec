using System;
using System.Linq;
using NSpec.Domain;
using NUnit.Framework;
using NSpec.Tests.WhenRunningSpecs.Exceptions;
using System.Reflection;
using FluentAssertions;

namespace NSpec.Tests
{
    [TestFixture]
    [Category("Context")]
    public class describe_Context
    {
        [Test]
        public void it_should_make_a_sentence_from_underscored_context_names()
        {
            new Context("i_love_underscores").Name.Should().Be("i love underscores");
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

            child.AddExample(new ExampleBaseWrap { Exception = new KnownException() });

            var parent = new Context("parent");

            parent.AddContext(child);

            parent.Failures().Count().Should().Be(1);
        }
    }

    [TestFixture]
    [Category("Context")]
    public class when_creating_act_contexts_for_derived_class
    {
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

            instance.actResult.Should().Be("parentchild");
        }

        ClassContext childContext, parentContext;

        child_act instance;
    }

    [TestFixture]
    [Category("Context")]
    public class when_creating_contexts_for_derived_classes
    {
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
            parentContext.Name.Should().Be(typeof(parent_before).Name.Replace("_", " "));
        }

        [Test]
        public void it_should_have_the_child_as_a_context()
        {
            parentContext.Contexts.First().Name.Should().Be(typeof(child_before).Name.Replace("_", " "));
        }

        private ClassContext childContext;

        private DefaultConventions conventions;

        private ClassContext parentContext;
    }

    [TestFixture]
    [Category("Context")]
    public class when_creating_before_contexts_for_derived_class
    {
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

            instance.beforeResult.Should().Be("parentchild");
        }

        ClassContext childContext, parentContext;

        child_before instance;
    }

    public abstract class trimming_contexts
    {
        protected Context rootContext;

        [SetUp]
        public void SetupBase()
        {
            rootContext = new Context("root context");
        }

        public Context GivenContextWithNoExamples()
        {
            return new Context("context with no example");
        }

        public Context GivenContextWithExecutedExample()
        {
            var context = new Context("context with example");
            context.AddExample(new ExampleBaseWrap("example"));
            context.Examples[0].HasRun = true;

            return context;
        }
    }

    [TestFixture]
    [Category("Context")]
    public class trimming_unexecuted_contexts_one_level_deep : trimming_contexts
    {
        Context contextWithExample;

        Context contextWithoutExample;

        [SetUp]
        public void given_nested_contexts_with_and_without_executed_examples()
        {
            contextWithoutExample = GivenContextWithNoExamples();

            rootContext.AddContext(contextWithoutExample);

            contextWithExample = GivenContextWithExecutedExample();

            rootContext.AddContext(contextWithExample);

            rootContext.Contexts.Count().Should().Be(2);

            rootContext.TrimSkippedDescendants();
        }

        [Test]
        public void it_contains_context_with_example()
        {
            rootContext.AllContexts().Should().Contain(contextWithExample);
        }

        [Test]
        public void it_doesnt_contain_empty_context()
        {
            rootContext.AllContexts().Should().NotContain(contextWithoutExample);
        }
    }

    [TestFixture]
    [Category("Context")]
    public class trimming_unexecuted_contexts_two_levels_deep : trimming_contexts
    {
        Context childContext;

        Context parentContext;

        public void GivenContextWithAChildContextThatHasExample()
        {
            parentContext = GivenContextWithNoExamples();

            childContext = GivenContextWithExecutedExample();

            parentContext.AddContext(childContext);

            rootContext.AddContext(parentContext);

            rootContext.AllContexts().Should().Contain(parentContext);
        }

        public void GivenContextWithAChildContextThatHasNoExample()
        {
            parentContext = GivenContextWithNoExamples();

            childContext = GivenContextWithNoExamples();

            parentContext.AddContext(childContext);

            rootContext.AddContext(parentContext);

            rootContext.AllContexts().Should().Contain(parentContext);
        }

        [Test]
        public void it_keeps_all_contexts_if_examples_exists_at_level_2()
        {
            GivenContextWithAChildContextThatHasExample();

            rootContext.TrimSkippedDescendants();

            rootContext.AllContexts().Should().Contain(parentContext);

            rootContext.AllContexts().Should().Contain(childContext);
        }

        [Test]
        public void it_removes_all_contexts_if_no_child_context_has_examples()
        {
            GivenContextWithAChildContextThatHasNoExample();

            rootContext.TrimSkippedDescendants();

            rootContext.AllContexts().Should().NotContain(parentContext);

            rootContext.AllContexts().Should().NotContain(childContext);
        }
    }

    [TestFixture]
    [Category("Context")]
    [Category("BareCode")]
    public class when_bare_code_throws_in_class_context
    {
        public class CtorThrowsSpecClass : nspec
        {
            readonly object someTestObject = DoSomethingThatThrows();

            public void method_level_context()
            {
                before = () => { };

                it["should pass"] = () => { };
            }

            static object DoSomethingThatThrows()
            {
                var specEx = new KnownException("Bare code threw exception");

                SpecException = specEx;

                throw specEx;
            }

            public static Exception SpecException;

            public static string TypeFullName = typeof(CtorThrowsSpecClass).FullName;
            public static string ExceptionTypeName = typeof(KnownException).Name;
        }

        [SetUp]
        public void setup()
        {
            var specType = typeof(CtorThrowsSpecClass);

            classContext = new ClassContext(specType);

            var methodInfo = specType.GetTypeInfo().GetMethod("method_level_context");

            var methodContext = new MethodContext(methodInfo);

            classContext.AddContext(methodContext);
        }

        [Test]
        public void building_should_not_throw()
        {
            Assert.DoesNotThrow(() => classContext.Build());
        }

        [Test]
        public void it_should_add_example_named_after_exception()
        {
            classContext.Build();

            string actual = classContext.AllExamples().Single().FullName();

            actual.Should().Contain(CtorThrowsSpecClass.ExceptionTypeName);
        }

        ClassContext classContext;
    }

    [TestFixture]
    [Category("Context")]
    [Category("BareCode")]
    public class when_bare_code_throws_in_nested_context
    {
        public class NestedContextThrowsSpecClass : nspec
        {
            public void method_level_context()
            {
                context["sub level context"] = () =>
                {
                    DoSomethingThatThrows();

                    before = () => { };

                    it["should pass"] = () => { };
                };
            }

            void DoSomethingThatThrows()
            {
                throw new KnownException("Bare code threw exception");
            }

            public static string ExceptionTypeName = typeof(KnownException).Name;
        }

        [SetUp]
        public void setup()
        {
            var specType = typeof(NestedContextThrowsSpecClass);

            classContext = new ClassContext(specType);

            var methodInfo = specType.GetTypeInfo().GetMethod("method_level_context");

            var methodContext = new MethodContext(methodInfo);

            classContext.AddContext(methodContext);
        }

        [Test]
        public void building_should_not_throw()
        {
            Assert.DoesNotThrow(() => classContext.Build());
        }

        [Test]
        public void it_should_add_example_named_after_exception()
        {
            classContext.Build();

            string actual = classContext.AllExamples().Single().FullName();

            actual.Should().Contain(NestedContextThrowsSpecClass.ExceptionTypeName);
        }

        ClassContext classContext;
    }
}
