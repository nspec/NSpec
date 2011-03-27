using NSpec;
using System.Collections.Generic;

class simple_before : nspec
{
    List<int> ints;

    void list_manipulation()
    {
        before = () => ints = new List<int>();

        it["the ints collection should not be null"] = () => ints.should_not_be_null();
    }
}