using NSpec;

namespace NSpecSpecs.ClassContextBug
{
    class Grand_Parent : nspec
    {
        public string TestValue;

        void before_each()
        {
            this.TestValue = "Grand Parent";
        }

        void act_each()
        {
            this.TestValue = this.TestValue + "!!!";
        }

        void Grand_Parent_Context()
        {
            it["TestValue should be \"Grand Parent!!!\""] = () => TestValue.should_be("Grand Parent!!!");
        }
    }

    class Parent : Grand_Parent
    {
        void before_each()
        {
            this.TestValue += "." + "Parent";
        }

        void act_each()
        {
            this.TestValue = this.TestValue + "@@@";
        }

        void Parent_Context()
        {
            it["TestValue should be \"Grand Parent.Parent!!!@@@\""] = () => TestValue.should_be("Grand Parent.Parent!!!@@@");
        }
    }

    class Child : Parent
    {
        void before_each()
        {
            this.TestValue += "." + "Child";
        }

        void act_each()
        {
            this.TestValue = this.TestValue + "###";
        }

        void Child_Context()
        {
            it["TestValue should be \"Grand Parent.Parent.Child!!!@@@###\""] = () => TestValue.should_be("Grand Parent.Parent.Child!!!@@@###");
        }
    }
}