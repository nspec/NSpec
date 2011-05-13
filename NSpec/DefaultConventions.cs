using System.Text.RegularExpressions;
using NSpec.Domain;

namespace NSpec
{
    public class DefaultConventions : Conventions
    {
        public override void SpecifyConventions(ConventionSpecification specification)
        {
            specification.SetBefore(new Regex("before_each", RegexOptions.IgnoreCase));

            specification.SetAct(new Regex("act_each", RegexOptions.IgnoreCase));

            specification.SetExample(new Regex("(^it_)|(^specify_)", RegexOptions.IgnoreCase));

            specification.SetContext(new Regex("_", RegexOptions.IgnoreCase));
        }
    }
}
