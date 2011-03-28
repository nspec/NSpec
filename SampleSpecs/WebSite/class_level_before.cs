using System.Collections.Generic;
using NSpec;

class class_level_before : nspec
{
    List<int> ints;

    void before_each()
    {
        ints = new List<int>();
    }

    void list_manipulations()
    {
        specify = () => ints.Count.should_be(0);

        context["number in collection"] = () =>
        {
            before = () => ints.Add(15);

            specify = () => ints.Count.should_be(1);
        };
    }
}