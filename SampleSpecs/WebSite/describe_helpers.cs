using NSpec;
using SampleSpecs.WebSite;

public class describe_helpers : nspec
{
    void when_making_tea()
    {
        context["that is 210 degrees"] = () =>
        {
            before = () => tea = MakeTea(210);
            it["should be hot"] = () => tea.Taste().should_be("hot");
        };
        context["that is 90 degrees"] = () =>
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