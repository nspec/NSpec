using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Domain.Extensions;
using System.Text.RegularExpressions;

namespace NSpec.Domain
{
    public class SpecFinder : ISpecFinder
    {
        private readonly string classFilter;

        public IEnumerable<Type> SpecClasses()
        {
            var regex = new Regex(classFilter);

            return Types
                .Where(t => t.IsClass
                    && BaseTypes(t).Any(s => s == typeof(nspec))
                    && t.Methods().Count() > 0
                    && (string.IsNullOrEmpty(classFilter) || regex.IsMatch(t.FullName)));
        }

        public IEnumerable<Type> BaseTypes(Type type)
        {
            var types = new List<Type>();

            var currentType = type.BaseType;

            while (currentType != null)
            {
                types.Add(currentType);
                currentType = currentType.BaseType;
            }

            return types;
        }

        public SpecFinder()
        {
        }

        public SpecFinder(string specDLL, IReflector reflector, string classFilter = "")
            : this()
        {
            this.classFilter = classFilter;
            Types = reflector.GetTypesFrom(specDLL);
        }

        public Type[] Types { get; set; }
    }

    public interface ISpecFinder
    {
        IEnumerable<Type> SpecClasses();
    }
}