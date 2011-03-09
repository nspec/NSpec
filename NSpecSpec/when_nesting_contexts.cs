using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;
using NSpec;

namespace NSpecSpec
{
    public class when_nesting_contexts: spec
    {
        public void when_adding_a_subcontext()
        {
            var parent = new Context("parent");

            var child = new Context("child");

            parent.AddContext(child);

            specify["the childs Parent property should be set"] = () => child.Parent.should_be(parent);
        }
    }
}