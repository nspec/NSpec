using System.Linq;
using NSpec;
using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_getting_field_level_befores
    {
        [Test]
        public void should_get_the_field_named_each_declared_as_before_dynamic()
        {
            var instance = new parent();

            var beforeAction = typeof(parent).GetBefore();

            beforeAction(instance);

            instance.beforeResult.should_be("parent");
        }

        [Test]
        public void should_get_all_the_field_befores_from_ancestors()
        {
            var instance = new child();

            var befores = typeof(child).GetBefores();

            befores.Count().should_be(2);

            befores.Do(b => b(instance));
            
            instance.beforeResult.should_be("parentchild");
        }

        [Test]
        public void should_create_contexts_with_the_befores()
        {
            var context = typeof(child).GetContexts();

            ExecuteBefore(context, new child()).should_be("parent");

            ExecuteBefore(context.Contexts.First(), new child()).should_be("child");
        }

        private string ExecuteBefore(Context context, child instance)
        {
            context.SetInstanceContext(instance);

            context.Before();

            return instance.beforeResult;
        }

        [Test]
        public void should_create_contexts()
        {
            var context = typeof(child).GetContexts();

            context.Name.should_be("parent");

            context.Contexts.First().Name.should_be("child");
        }

        [Test]
        public void setting_the_parent_context_instanceContext_should_set_childrens()
        {
            var context = typeof(child).GetContexts();

            var instance = new child();

            context.SetInstanceContext(instance);

            context.Contexts.First().Before();

            instance.beforeResult.should_be("child");
        }
    }

    public class child : parent
    {
        before<dynamic> each = childSpec => childSpec.beforeResult += "child";
    }

    public class parent : spec
    {
        before<dynamic> each = beforeClass => beforeClass.beforeResult = "parent";
        public string beforeResult;
    }
}