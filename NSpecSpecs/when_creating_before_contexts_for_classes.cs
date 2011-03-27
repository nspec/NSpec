using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecNUnit
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

    [TestFixture]
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