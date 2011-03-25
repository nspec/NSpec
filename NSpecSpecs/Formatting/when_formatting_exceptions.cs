using System;
using NSpec;
using NSpec.Domain.Extensions;
using NUnit.Framework;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_formatting_exceptions
    {
        [Test]
        public void should_collapse_spaces()
        {
            CleanExceptionMessage("double space  should collapse").should_be("double space should collapse");
        }

        [Test]
        public void should_trim_spaces_and_new_lines()
        {
            CleanExceptionMessage(" {0}trimmed   {0}".With(Environment.NewLine)).should_be("trimmed");
        }

        private string CleanExceptionMessage(string message)
        {
            return new Exception(message).CleanMessage();
        }
    }
}