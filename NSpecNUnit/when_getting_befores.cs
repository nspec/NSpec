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

            typeof(BeforeClass).GetBefore<BeforeClass>()(instance);

            instance.beforeResult.should_be("BeforeClass");
        }
    }
    public class BeforeClass : spec
    {
        before<dynamic> each = specClass => specClass.beforeResult = "BeforeClass";
        public string beforeResult;

        public void public_method() { }
        private void private_method() { }
    }
}