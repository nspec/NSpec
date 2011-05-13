using System;
using System.Text.RegularExpressions;

namespace NSpec
{
    public class ConventionSpecification
    {
        public void SetBefore(string startsWith)
        {
            SetBefore(new Regex("^" + startsWith));
        }

        public void SetBefore(Regex regex)
        {
            Before = regex;
        }

        public void SetAct(string startsWith)
        {
            SetAct(new Regex("^" + startsWith));
        }

        public void SetAct(Regex regex)
        {
            Act = regex;
        }

        public void SetExample(string startsWith)
        {
            SetExample(new Regex("^" + startsWith));
        }

        public void SetExample(Regex regex)
        {
            Example = regex;
        }

        public void SetContext(string startsWith)
        {
            SetContext(new Regex("^" + startsWith));
        }

        public void SetContext(Regex regex)
        {
            Context = regex;
        }

        public Regex Before { get; private set; }

        public Regex Act { get; private set; }

        public Regex Example { get; private set; }

        public Regex Context { get; private set; }
    }
}
