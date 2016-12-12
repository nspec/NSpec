using System;
using System.Collections.Generic;
using System.Linq;
using NSpec;
using FluentAssertions;

public abstract class describe_ICollection : nspec
{
    protected ICollection<string> collection;

    void adding_to_collection()
    {
        before = () => collection.Add("Item 1");

        it["contains the entry"] = () =>
            collection.Contains("Item 1").Should().Be(true);
    }
}

public class describe_LinkedList : describe_ICollection
{
    void before_each()
    {
        linkedList = new LinkedList<string>();
        collection = linkedList;
    }

    void specific_actions()
    {
        before = () => collection.Add("Item 1");

        it["can add an item at the begining with ease"] = () =>
        {
            linkedList.AddFirst("Item 2");
            linkedList.First.Value.Should().Be("Item 2");
        };
    }
    LinkedList<string> linkedList;
}

public class describe_List : describe_ICollection
{
    void before_each()
    {
        list = new List<string>();
        collection = list;
    }

    void specific_actions()
    {
        before = () => collection.Add("Item 1");

        it["an item can be referenced by index"] = () =>
            list[0].Should().Be("Item 1");
    }
    List<string> list;
}

public static class describe_ICollection_output
{
    public static string Output = @"
describe LinkedList
  adding to collection
    contains the entry (__ms)
  specific actions
    can add an item at the begining with ease (__ms)

describe List
  adding to collection
    contains the entry (__ms)
  specific actions
    an item can be referenced by index (__ms)

4 Examples, 0 Failed, 0 Pending
";
    public static int ExitCode = 0;
}
