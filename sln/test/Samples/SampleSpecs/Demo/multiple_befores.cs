using FluentAssertions;
using NSpec;
using System.Collections.Generic;

class multiple_befores : nspec
{
    List<int> ints;

    void list_manipulation()
    {
        before = () => ints = new List<int>();

        it["the ints collection should not be null"] = () => ints.Should().NotBeNull();

        context["one item in list"] = () =>
        {
            before = () => ints.Add(99);

            it["should have 1 item in list"] = () => ints.Count.Should().Be(1);

            it["should contain the number 99"] = () => ints.Should().Contain(99);

            context["another item in list"] = () =>
            {
                before = () => ints.Add(26);

                it["should have 2 items in list"] = () => ints.Count.Should().Be(2);

                it["should contain the number 26"] = () => ints.Should().Contain(26);
            };
        };
    }
}