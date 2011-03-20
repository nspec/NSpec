using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec.Domain;
using NSpec.Assertions;
using System.Linq.Expressions;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_parsing_expressions
    {
        [Test]
        public void should_clear_quotes()
        {
            Parsing(() => "hello".should_be("hello")).should_be("hello should be hello");
        }

        public string Parsing(Expression<Action> expression)
        {
            return ActionRegister.Parse(expression);
        }
    }
}
