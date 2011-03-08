using NSpec.Extensions;
using NSpec.Interpreter.Indexer;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_getting_befores
    {
        [Test]
        public void should_get_the_field_named_each_declared_as_before_dynamic()
        {
            var instance = new BeforeClass();

            typeof(BeforeClass).GetBefore()(instance);

            instance.beforeResult.should_be("BeforeClass");
        }

        [Test,Ignore]
        public void should_get_the_field_()
        {
            var instance = new ChildSpec();

            typeof(ChildSpec).GetBefore()(instance);

            instance.beforeResult.should_be("BeforeClassChildSpec");
        }
    }
    public class ChildSpec : BeforeClass
    {
        before<dynamic> each = childSpec => childSpec.beforeResult += "ChildSpec";
    }
    public class BeforeClass : spec
    {
        before<dynamic> each = beforeClass => beforeClass.beforeResult = "BeforeClass";
        public string beforeResult;

        public void public_method() { }
        private void private_method() { }
    }
}