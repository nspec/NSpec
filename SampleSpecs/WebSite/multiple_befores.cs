using NSpec;
using NSpec.Extensions;
using System.Collections.Generic;


public class multiple_befores : spec
{
    private List<int> ints = null;

    public void list_manipulation()
    {
        before = () => ints = new List<int>();

        it["the ints collection should not be null"] = () => ints.should_not_be_null();

        context["one item in list"] = () =>
        {
            before = () => ints.Add(99);

            it["should have 1 item in list"] = () => ints.Count.should_be(1);

            it["should contain the number 99"] = () => ints.should_contain(99);

            context["another item in list"] = () =>
            {
                before = () => ints.Add(26);

                it["should have 2 items in list"] = () => ints.Count.should_be(2);

                it["should contain the number 26"] = () => ints.should_contain(26);
            };
        };
    }
}