using NSpec;

class describe_pending : nspec
{
    void when_creating_pending_specifications()
    {
        it["pending spec"] = todo;
        //or just add an 'x' at the beginning of a specification that isn't quite ready
        xit["\"\" should be \"something else\""] = () => "".should_be("something else");
    }
}

public static class describe_pending_output
{
    public static string Output = @"
describe pending
  when creating pending specifications
    pending spec - PENDING
    """" should be ""something else"" - PENDING

2 Examples, 0 Failed, 2 Pending
";
}