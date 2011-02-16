using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpecSpec
{
    public class when_executing_a_context : spec
    {
        private Context context;

        public void a_context()
        {
            before.each = () => context = new Context("test");

            with["no examples of its own, but a subcontext with an example"] = () =>
            {
                before.each = () =>
                {
                    var sub = new Context("subContext");

                    sub.AddExample(new Example("example"));

                    context.Contexts.Add(sub);
                };

                specify(() => context.ToString().is_not_null_or_empty());
            };

            given["before all"] = () =>
            {
                var beforeAllCount = 0;

                context.Before = () => beforeAllCount++;

                context.BeforeFrequency = "all";

                when["the Befores run the first time"] = () =>
                {
                    context.Befores();

                    specify(() => beforeAllCount.ShouldBe(1));

                    when["the Befores run the second time"] = () =>
                    {
                        context.Befores();

                        specify(() => beforeAllCount.ShouldBe(1));
                    };
                };
            };
        }
    }
}