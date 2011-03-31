using NSpec;

namespace SampleSpecs.WebSite
{
    public class describe_helpers : nspec
    {
        void when_making_tea()
        {
            context["that is too hot"] = () =>
            {
                before = () => tea = MakeTea(215);
                it["should be hot"] = () => tea.Taste().should_be("hot");
            };

            context["that is too cold"] = ()=>
            {
                before = () => tea = MakeTea(90);
                it["should be cold"] = () => tea.Taste().should_be("cold");
            };
        }

        Tea MakeTea(int temperature)
        {
            return new Tea(temperature);
        }
        Tea tea;
    }
}