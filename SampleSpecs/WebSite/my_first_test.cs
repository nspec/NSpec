using NSpec;

class my_first_test : spec
{
    public void given_i_am_awesome()
    {
        it["should pass"] = () => "Hello World".should_be("Hello World");
    }
}