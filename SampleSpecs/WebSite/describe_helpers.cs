using NSpec;

public class describe_helpers : nspec
{
    void when_making_tea()
    {
        context["that is 210 degrees"] = () =>
        {
            before = () => MakeTea(210);
            it["should be hot"] = () => tea.Taste().should_be("hot");
        };
        context["that is 90 degrees"] = () =>
        {
            before = () => MakeTea(90);
            it["should be cold"] = () => tea.Taste().should_be("cold");
        };
    }
    void MakeTea(int temperature)
    {
        tea = new Tea(temperature);
    }
    Tea tea;
}