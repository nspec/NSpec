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