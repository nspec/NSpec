using System;
using System.Linq;
using NSpec;
using NUnit.Framework;
using NSpec.Domain.Extensions;
using NSpec.Domain;

namespace NSpecNUnit
{
    [TestFixture]
    public class describe_expected_exception
    {
        private class SpecClass : nspec
        {
            public void method_level_context()
            {
                it["should throw exception"] = expect<InvalidOperationException>(() => { throw new InvalidOperationException(); });

                it["should fail if no exception thrown"] = expect<InvalidOperationException>(() => { });

                it["should fail if wrong exception thrown"] = expect<InvalidOperationException>(() => { throw new ArgumentException(); });
            }
        }

        [SetUp]
        public void setup()
        {
            classContext = new Context(typeof(SpecClass));

            var method = typeof(SpecClass).Methods().Single(m => m.Name == "method_level_context");

            methodContext = new Context(method);

            classContext.AddContext(methodContext);

            classContext.Run();
        }

        [Test]
        public void given_exception_is_thrown_should_not_fail()
        {
            TheExample("should throw exception").Exception.should_be(null);
        }

        [Test]
        public void given_exception_not_thrown_should_fail()
        {
            TheExample("should fail if no exception thrown").Exception.GetType().should_be(typeof(ExceptionNotThrown));
        }

        [Test]
        public void given_wrong_exception_should_fail()
        {
            TheExample("should fail if wrong exception thrown").Exception.GetType().should_be(typeof(ExceptionNotThrown));
            TheExample("should fail if wrong exception thrown").Exception.Message.should_be("Exception of type InvalidOperationException was not thrown.");
        }

        private Example TheExample(string name)
        {
            return classContext.Contexts.First().AllExamples().Single(s => s.Spec == name);
        }

        private Context classContext;
        private Context methodContext;
    }
}