using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecNUnit
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

    [TestFixture]
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
}