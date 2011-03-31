using System.Collections.Generic;
using NSpec;

class describe_class_level_before : nspec
{
    List<int> ints;

    //by defining a method called before_each, nspec will execute this class
    //level method before each test
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