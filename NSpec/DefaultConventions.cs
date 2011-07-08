using System;
using System.Text.RegularExpressions;
using NSpec.Domain;

namespace NSpec
{
    [Serializable]
    public class DefaultConventions : Conventions
    {
        public override void SpecifyConventions(ConventionSpecification specification)
        {
            specification.SetBefore(RegexInsensitive("^before_each"));

            specification.SetAct(RegexInsensitive("^act_each"));

            specification.SetExample(RegexInsensitive("(^it_)|(^specify_)"));

            specification.SetContext(RegexInsensitive("_"));
        }

        Regex RegexInsensitive(string pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}
