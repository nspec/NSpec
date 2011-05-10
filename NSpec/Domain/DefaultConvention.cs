using System.Text.RegularExpressions;

namespace NSpec.Domain
{
    public class DefaultConvention : Conventions
    {
        public override void SpecifyConventions(ConventionSpecification specification)
        {
            specification.SetBefore(new Regex("before_each|BeforeEach"));

            specification.SetAct(new Regex("act_each|ActEach"));

            specification.SetExample(new Regex("(^[iI]t[_A-Z])|(^[sS]pecify)"));

            specification.SetContext(new Regex(""));
        }
    }
}
