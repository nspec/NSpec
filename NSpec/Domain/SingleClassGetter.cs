using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.Domain
{
    [Serializable]
    public class SingleClassGetter : ISpecFinder
    {
        private Type _typeToReturn;

        public SingleClassGetter(Type type)
        {
            _typeToReturn = type;
        }

        public IEnumerable<Type> SpecClasses()
        {
            return new List<Type> { _typeToReturn };
        }
    }
}
