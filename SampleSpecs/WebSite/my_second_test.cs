using NSpec;
using NSpec.Assertions;
using NSpec.Extensions;


public class my_second_test : spec
{
    public void i_am_a_method_level_context()
    {
        it["should execute at this level"] = () => "Hello World".should_be("Hello World");

        context["i am in a sub context"] = () =>
        {
            it["should execute within my sub context"] = () => "Hello World".should_be("Hello World");

            context["nspec is so cool"] = () =>
            {
                it["should execute in a sub sub context"] = () => "Hello World".should_be("Hello World");
            };

            context["i'm given sibling contexts"] = () =>
            {
                it["should execute in a sibling context"] = () => "Hi".should_be("Hi");
            };
        };
    }
}


