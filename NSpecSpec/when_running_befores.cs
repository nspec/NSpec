using System.Collections.Generic;
using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;
using NSpec;

namespace NSpecSpec
{
    public class when_running_befores : spec
    {
        List<string> executionOrder;

        before<dynamic> each = c => c.executionOrder = new List<string>();
        private Context sibling;

        public void parents_before_should_execute_before_siblings()
        {
            executionOrder = new List<string>();

            describe["a parent with no before"] = () =>
            {
                var parent = new Context("parent");

                var grandParent = ContextWithBeforeEach("grandParent");

                grandParent.AddContext(parent);

                //var firstChild = SubContextWithBeforeEach(parent, "firstchild");

                sibling = SubContextWithBeforeEach(parent, "sibling");

                //firstChild.Befores();

                //given["the first child's befores have executed"] = () => firstChild.Befores();
                context["the siblings befores execute"] = () =>
                {
                    before = () => sibling.Befores();

                    specify(() => executionOrder.should_be(new[] { "grandParent", "sibling" }));
                };
            };
        }

        Context SubContextWithBeforeEach(Context parent, string name)
        {
            var context = ContextWithBeforeEach(name);

            parent.AddContext(context);

            return context;
        }

        private Context ContextWithBeforeEach(string name)
        {
            var context = new Context(name);

            context.Before = () => executionOrder.Add(name);

            return context;
        }
    }
}