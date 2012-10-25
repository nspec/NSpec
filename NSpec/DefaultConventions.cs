using System.Text.RegularExpressions;
using NSpec.Domain;

namespace NSpec
{
    public class DefaultConventions : Conventions
    {
        public override void SpecifyConventions(ConventionSpecification specification)
        {
            specification.SetBefore(RegexInsensitive("^before_each"));

            specification.SetBeforeAll(RegexInsensitive("^before_all"));

            specification.SetAct(RegexInsensitive("^act_each"));

            specification.SetAfter(RegexInsensitive("^after_each"));

            specification.SetAfterAll(RegexInsensitive("^after_all"));

            specification.SetExample(RegexInsensitive("(^it_)|(^specify_)"));

            specification.SetContext(RegexInsensitive("_"));
            //anything that doesn't match is considered a helper method and is never directly invoked
        }

        Regex RegexInsensitive(string pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}