using NSpec;
using NSpec.Domain;
using NSpecSpecs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSpecNUnit
{
    [TestFixture]
    [Category("Example")]
    public class when_parsing_expressions
    {
        [Test]
        public void should_clear_quotes()
        {
            new Example(() => "hello".should_be("hello")).Spec.should_be("hello should be hello");
        }

        // no 'specify' available for AsyncExample, hence no way to test that on AsyncExample
    }

    [TestFixture]
    [Category("Example")]
    public class describe_ExampleBase
    {
        [Test]
        public void should_concatenate_its_contexts_name_into_a_full_name()
        {
            var context = new Context("context name");

            var example = new ExampleBaseWrap("example name");

            context.AddExample(example);

            example.FullName().should_be("context name. example name.");
        }

        [Test]
        public void should_be_marked_as_pending_if_parent_context_is_pending()
        {
            var context = new Context("pending context", null, isPending: true);

            var example = new ExampleBaseWrap("example name");

            context.AddExample(example);

            example.Pending.should_be_true();
        }

        [Test]
        public void should_be_marked_as_pending_if_any_parent_context_is_pending()
        {
            var parentContext = new Context("parent pending context", null, isPending: true);
            var context = new Context("not pending");
            var example = new ExampleBaseWrap("example name");

            parentContext.AddContext(context);

            context.AddExample(example);

            example.Pending.should_be_true();
        }
    }
}