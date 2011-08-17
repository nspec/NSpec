using NSpec;

namespace NSpecSpecs.ClassContextBug
{
    public class Grand_Parent : nspec
    {
        public void Grand_Parent_Context()
        {
            it["Grand Parent - Do something"] = todo;
        }
    }

    public class Parent : Grand_Parent
    {
        public void Parent_Context()
        {
            it["Parent - Do something"] = todo;
        }
    }

    public class Child : Parent
    {
        public void Child_Context()
        {
            it["Child - Do something"] = todo;
        }
    }
}