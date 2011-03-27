using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecNUnit
{
    public class child : parent
    {
        public void before_each()
        {
            beforeResult += "child";
        }
    }

    public class parent : nspec
    {
        public string beforeResult;
        public void before_each()
        {
            beforeResult = "parent";
        }
    }

    [TestFixture]
    public class when_creating_contexts_for_a_class
    {
        [SetUp]
        public void setup()
        {
            context = typeof(child).RootContext();
        }

        [Test]
        public void the_root_context_should_be_the_be_the_parent()
        {
            context.Name.should_be(typeof(parent).Name);
        }

        [Test]
        public void it_should_have_the_child_as_a_context()
        {
            context.Contexts.First().Name.should_be(typeof(child).Name);
        }

        private Context context;
    }

    [TestFixture]
    public class when_setting_the_instance_context_on_the_child
    {
        [SetUp]
        public void setup()
        {
            context = typeof(child).RootContext();

            instance = new child();

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
        private child instance;
    }
}