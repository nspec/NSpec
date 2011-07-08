using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Domain.Extensions;
using System.Text.RegularExpressions;

namespace NSpec.Domain
{
    [Serializable]
    public class SpecFinder : ISpecFinder
    {
        private readonly string classFilter;

        public IEnumerable<Type> SpecClasses()
        {
            var regex = new Regex(classFilter);

            var leafTypes = 
                Types.Where(t => t.IsClass
                    && BaseTypes(t).Any(s => s == typeof(nspec))
                    && t.Methods().Count() > 0
                    && (string.IsNullOrEmpty(classFilter) || regex.IsMatch(t.FullName)));

            var finalList = new List<Type>();
            finalList.AddRange(leafTypes);

            foreach (var leafType in leafTypes)
            {
                finalList.AddRange(BaseTypes(leafType));
            }

            return finalList.Distinct(new TypeComparer()).Where(s => s != typeof(nspec) && s != typeof(object));
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

    public class TypeComparer : IEqualityComparer<Type>
    {

        public bool Equals(Type x, Type y)
        {
            return x.FullName == y.FullName;
        }

        public int GetHashCode(Type obj)
        {
            return obj.FullName.GetHashCode();
        }
    }

    public interface ISpecFinder
    {
        IEnumerable<Type> SpecClasses();
    }
}