using NSpec;

public class describe_class_level : nspec
{
    //before, act, and it can also be declared at the class level like so
    void before_each()
    {
        sequence = "arrange, ";
    }
    void act_each()
    {
        sequence += "act";
    }

    //prefixing a method with "it_" or "specify_"
    //will tell nspec to treat the method as an example
    void specify_given_befores_and_acts_run_in_the_correct_sequence()
    {
        sequence.should_be("arrange, act");
    }

    string sequence;
}

public static class describe_class_level_output
{
    public static string Output = @"
describe class level
  specify given befores and acts run in the correct sequence (__ms)

1 Examples, 0 Failed, 0 Pending
";
    public static int ExitCode = 0;
}
