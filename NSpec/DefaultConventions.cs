using System.Text.RegularExpressions;
using NSpec.Domain;

namespace NSpec
{
    public class DefaultConventions : Conventions
    {
        public override void SpecifyConventions(ConventionSpecification specification)
        {
            specification.SetBefore(RegexInsensitive("^before_each"));

            specification.SetAct(RegexInsensitive("^act_each"));

            specification.SetAfter(RegexInsensitive("^after_each"));

            specification.SetExample(RegexInsensitive("(^it_)|(^specify_)"));

            specification.SetContext(RegexInsensitive("_"));
        }

        Regex RegexInsensitive(string pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}
