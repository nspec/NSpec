using NSpec;

class my_first_spec : nspec
{
    void given_the_world_has_not_come_to_an_end()
    {
        it["Hello World should be Hello World"] = () => "Hello World".should_be("Hello World");
    }
}