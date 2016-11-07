using FluentAssertions;
using NSpec;
using System.Threading.Tasks;

public class describe_async_helpers : nspec
{
    void when_making_tea()
    {
        context["that is 210 degrees"] = () =>
        {
            beforeAsync = async () => await MakeTeaAsync(210);
            it["should be hot"] = () => tea.Taste().Should().Be("hot");
        };
        context["that is 90 degrees"] = () =>
        {
            beforeAsync = async () => await MakeTeaAsync(90);
            it["should be cold"] = () => tea.Taste().Should().Be("cold");
        };
    }

    //helper methods do not have underscores
    async Task MakeTeaAsync(int temperature)
    {
        tea = await Task.Run(() => new Tea(temperature));
    }

    Tea tea;
}

public static class describe_async_helpers_output
{
    public static string Output = @"
describe async helpers
  when making tea
    that is 210 degrees
      should be hot (__ms)
    that is 90 degrees
      should be cold (__ms)

2 Examples, 0 Failed, 0 Pending
";

    public static int ExitCode = 0;
}
