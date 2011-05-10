using System;
using System.Text.RegularExpressions;

namespace NSpec.Domain
{
    public class UnderScore : Conventions
    {
        public override void SpecifyConventions(ConventionSpecification specification)
        {
            specification.SetBefore("before_each");

            specification.SetAct("act_each");

            specification.SetExample(new Regex("(^it_)|(^specify_)"));

            specification.SetContext(new Regex("_"));
        }
    }
}
