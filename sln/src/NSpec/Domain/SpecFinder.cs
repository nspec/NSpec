using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class SpecFinder : ISpecFinder
    {
        public virtual IEnumerable<Type> SpecClasses()
        {
            var regex = new Regex(filter);

            var leafTypes =
                Types.Where(t => t.GetTypeInfo().IsClass
                                 && !t.GetTypeInfo().IsAbstract
                                 && BaseTypes(t).Any(s => s == typeof(nspec))
                                 && (t.Methods().Any() || t.AsyncMethods().Any()));

            if (!string.IsNullOrEmpty(filter))
            {
                leafTypes = leafTypes.Where(t => regex.IsMatch(t.FullName));
            }

            var finalList = new List<Type>();
            finalList.AddRange(leafTypes);

            foreach (var leafType in leafTypes)
            {
                finalList.AddRange(BaseTypes(leafType));
            }

            return finalList.Distinct(new TypeComparer())
                .Where(s => s != typeof(nspec) && s != typeof(object) && !s.GetTypeInfo().IsAbstract);
        }

        public IEnumerable<Type> BaseTypes(Type type)
        {
            var types = new List<Type>();

            var currentType = type.GetTypeInfo().BaseType;

            while (currentType != null)
            {
                types.Add(currentType);
                currentType = currentType.GetTypeInfo().BaseType;
            }

            return types;
        }

        public SpecFinder(Type[] types, string filter = "")
        {
            this.filter = filter;
            Types = types;
        }

        public SpecFinder(IReflector reflector, string filter = "")
            : this(reflector.GetTypesFrom(), filter) {}

        string filter;

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