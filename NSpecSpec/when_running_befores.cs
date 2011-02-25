using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpecSpec
{
    public class when_running_befores : spec
    {
        private string executionOrder;

        public void parents_before_should_execute_before_siblings()
        {
            given["a parent with two children"] = () =>
            {
                var grandParent = new Context("grandParent");

                var parent = new Context("parent");

                var firstChild = new Context("child");

                var sibling = new Context("sibling");

                grandParent.Before = () => executionOrder += "grandParent";

                grandParent.BeforeFrequency = "each";

                //parent.Before = () => executionOrder += "parent";

                //parent.BeforeFrequency = "each";

                firstChild.Before = () => executionOrder += "child";

                firstChild.BeforeFrequency = "each";

                sibling.Before = () => executionOrder += "sibling";

                sibling.BeforeFrequency = "each";

                grandParent.AddContext(parent);

                parent.AddContext(firstChild);

                parent.AddContext(sibling);

                given["the first child's befores have executed"] = () => firstChild.Befores();

                when["the siblings befores execute"] = ()=>
                {
                    executionOrder = "";

                    sibling.Befores();
                        
                    specify(() => executionOrder.should_be("grandParentsibling"));
                };
            };
        }
    }
}