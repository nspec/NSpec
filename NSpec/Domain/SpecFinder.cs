using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class SpecFinder : ISpecFinder
    {
        private readonly string classFilter;

        public IEnumerable<Type> SpecClasses()
        {
            return Types
                .Where(t => t.IsClass
                    && BaseTypes(t).Any(s => s == typeof(spec))
                    && t.Methods(Except).Count() > 0
                    && (string.IsNullOrEmpty(classFilter) || t.Name == classFilter));
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
            Except = typeof(object).GetMethods().Select(m => m.Name).Union(new[] { "ClearExamples", "Examples", "set_Context", "get_Context" });
        }

        public SpecFinder(string specDLL, IReflector reflector, string classFilter="")
            : this()
        {
            this.classFilter = classFilter;
            Types = reflector.GetTypesFrom(specDLL);
        }

        public IEnumerable<string> Except { get; set; }

        public Type[] Types { get; set; }
    }

    public interface ISpecFinder
    {
        IEnumerable<Type> SpecClasses();
        IEnumerable<string> Except { get; set; }
    }
}