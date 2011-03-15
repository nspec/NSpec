using System.Linq;
using NSpec.Domain;
using NUnit.Framework;
using NSpec;
using NSpec.Interpreter.Indexer;
using NSpec.Extensions;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_finding_root_context
    {
        [Test]
        public void should_be_class_that_inherits_directly_from_spec_as_root_context()
        {
            TheRootContextFor<parent_spec>().Name.should_be("parent_spec");
        }

        [Test]
        public void should_be_root_class_that_inherits_directly_from_spec()
        {
            TheRootContextFor<child_spec>().Name.should_be("parent_spec");
        }

        [Test]
        public void derived_spec_class_should_be_contained_in_root_context_as_child()
        {
            var root = TheRootContextFor<child_spec>();

            root.Contexts.First().Name.should_be("child_spec");
        }

        [Test]
        public void should_capture_before_each_private_member_on_spec_class()
        {
            TheRootContextFor<parent_spec>().Before.should_not_be_null();
        }

        [Test]
        public void should_capture_before_each_private_member_on_derived_class()
        {
            var root = TheRootContextFor<child_spec>();

            root.Contexts.First().Before.should_not_be_null();
        }

        private Context TheRootContextFor<T>() where T : spec, new()
        {
            T spec = new T();

            var context = typeof(T).RootContext();

            context.SetInstanceContext(spec);

            return context;
        }
    }

    public class parent_spec : spec
    {
        before<object> each = d =>
        {

        };
    }

    public class child_spec : parent_spec
    {
        before<object> each = d =>{};
    }
}
