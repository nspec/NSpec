using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpec
{
    public class BeforeFinder
    {
        public IEnumerable<Action<object>> GetBefores(Type type) 
        {
            if (type.BaseType == typeof(spec))
                return new[] { GetBefore(type) };

            return GetBefores(type.BaseType).Concat( new []{GetBefore(type)});
        }

        public Action<object> GetBefore(Type type)
        {
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            before<dynamic> beforeEach = null;

            var instance = type.Instance<spec>();

            var eachField = fields.FirstOrDefault(f => f.Name.Contains("each"));

            if (eachField != null) beforeEach = eachField.GetValue(instance) as before<dynamic>;

            if (beforeEach != null) return t => beforeEach(t);

            return null;
        }
    }
}