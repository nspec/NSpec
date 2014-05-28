using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec.Domain;
using NSpec.Domain.Formatters;

namespace NSpec.MSTestExtensions.Attributes
{
    public class NSpecification : Attribute
    {
        private string _className;

        public NSpecification(string className)
        {
            _className = className;
            ShimSpecification();
        }

        public void ShimSpecification()
        {
            var types = GetType().Assembly.GetTypes();
            var finder = new SpecFinder(types, "");
            var builder = new ContextBuilder(finder, new Tags().Parse(_className), new DefaultConventions());
            var runner = new ContextRunner(builder, new ConsoleFormatter(), false);
            var results = runner.Run(builder.Contexts().Build());

            results.Failures().Count().should_be(0);
        }
    }
}
