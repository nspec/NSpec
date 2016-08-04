using NSpec;
using NSpec.Domain;
using NSpecSpecs.describe_RunningSpecs.Exceptions;
using NUnit.Framework;
using System.Linq;

namespace NSpecSpecs
{
    [TestFixture]
    [Category("MethodContext")]
    [Category("BareCode")]
    public class when_bare_code_throws
    {
        public class SpecClass : nspec
        {
            public void method_level_context()
            {
                DoSomethingThatThrows();

                before = () => { };

                it["should pass"] = () => { };
            }

            void DoSomethingThatThrows()
            {
                throw new KnownException("Bare code threw exception");
            }

            public static string ExceptionTypeName = typeof(KnownException).Name;
        }

        [SetUp]
        public void setup()
        {
            var specType = typeof(SpecClass);

            classContext = new ClassContext(specType);

            var methodInfo = specType.GetMethod("method_level_context");

            var methodContext = new MethodContext(methodInfo);

            classContext.AddContext(methodContext);
        }

        [Test]
        public void building_should_not_throw()
        {
            Assert.DoesNotThrow(() => classContext.Build());
        }

        [Test]
        public void it_should_add_example_named_after_exception()
        {
            classContext.Build();

            string actual = classContext.AllExamples().Single().FullName();

            actual.should_contain(SpecClass.ExceptionTypeName);
        }

        ClassContext classContext;
    }
}
