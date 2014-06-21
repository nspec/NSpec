using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.Domain
{
    /// <summary>
    /// Implements the ISpecFinder interface to return a single class to the builder
    /// </summary>
    [Serializable]
    public class SingleClassGetter : ISpecFinder
    {
        private Type _typeToReturn;

        /// <summary>
        /// Constructor takes the Type that should be returned
        /// </summary>
        /// <param name="type">The Type to return</param>
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
