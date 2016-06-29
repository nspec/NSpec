using NSpec;
using NSpec.Domain;
using NSpecSpecs.describe_RunningSpecs.Exceptions;
using NUnit.Framework;
using System.Linq;

namespace NSpecSpecs
{
    [TestFixture]
    [Category("MethodContext")]
    public class when_bare_code_throws
    {
        public class SpecClass : nspec
        {
            public void Given_some_context()
            {
                DoSomethingThatThrows();

                before = () => { };

                it["Should pass"] = () => { };
            }

            void DoSomethingThatThrows()
            {
                throw new KnownException("Bare code threw exception");
            }
        }

        [SetUp]
        public void setup()
        {
            var specType = typeof(SpecClass);

            classContext = new ClassContext(specType);

            var methodInfo = specType.GetMethod("Given_some_context");

            var methodContext = new MethodContext(methodInfo);

            classContext.AddContext(methodContext);
        }

        [Test]
        public void building_should_not_throw()
        {
            Assert.DoesNotThrow(() => classContext.Build());
        }

        [Test]
        public void it_should_add_example_named_after_context_and_exception()
        {
            string expected = "SpecClass. Given some context. Given_some_context throws an exception of type KnownException.";

            classContext.Build();

            string actual = classContext.AllExamples().Single().FullName();

            actual.should_be(expected);
        }

        ClassContext classContext;
    }
}
