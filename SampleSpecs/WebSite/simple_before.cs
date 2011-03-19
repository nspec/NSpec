using NSpec;
using NSpec.Extensions;
using System.Collections.Generic;


public class simple_before : spec
{
    private List<int> ints = null;

    public void list_manipulation()
    {
        before = () => ints = new List<int>();

        it["the ints collection should not be null"] = () => ints.should_not_be_null();
    }
}