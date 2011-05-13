using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec.Domain;
using NSpec;
using System.Linq.Expressions;

namespace NSpecSpecs
{
    [TestFixture]
    [Category("ExpressionParser")]
    public class when_parsing_expressions_with_quotes
    {
        [Test]
    	public void should_strip_quotes()
        {
            Parse(() => "hello".ShouldBe("hello")).ShouldBe("hello ShouldBe hello");
        }

        public string Parse(Expression<Action> expression)
        {
            return ExpressionParser.Parse(expression);
        }
    }
}
