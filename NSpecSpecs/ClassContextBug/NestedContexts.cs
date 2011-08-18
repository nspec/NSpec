using NSpec;

namespace NSpecSpecs.ClassContextBug
{
    public class Grand_Parent : nspec
    {
        public string TestValue;

        public void before_each()
        {
            this.TestValue = "Grand Parent";
        }
        public void act_each()
        {
            this.TestValue = this.TestValue + "!!!";
        }
        public void Grand_Parent_Context()
        {
            it["TestValue should be \"Grand Parent!!!\""] = () => TestValue.should_be( "Grand Parent!!!" );
        }
    }

    public class Parent : Grand_Parent
    {
        public void before_each()
        {
            this.TestValue += "." + "Parent";
        }
        public void act_each()
        {
            this.TestValue = this.TestValue + "@@@";
        }

        public void Parent_Context()
        {
            it["TestValue should be \"Grand Parent.Parent!!!@@@\""] = () => TestValue.should_be( "Grand Parent.Parent!!!@@@" );
        }
    }

    public class Child : Parent
    {
        public void before_each()
        {
            this.TestValue += "." + "Child";
        }
        public void act_each()
        {
            this.TestValue = this.TestValue + "###";
        }
        public void Child_Context()
        {
            it["TestValue should be \"Grand Parent.Parent.Child!!!@@@###\""] = () => TestValue.should_be( "Grand Parent.Parent.Child!!!@@@###" );
        }
    }
}