using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;
using NSpec;

namespace NSpecSpec
{
    public class when_executing_a_context : spec
    {
        private Context context;
        
        public void a_context()
        {
            before = () => context = new Context("test");

            describe["no examples of its own, but a subcontext with an example"] = () =>
            {
                before = () =>
                {
                    var sub = new Context("subContext");

                    sub.AddExample(new Example("example"));

                    context.AddContext(sub);
                };

                specify(() => context.ToString().is_not_null_or_empty());
            };

        }
    }
}