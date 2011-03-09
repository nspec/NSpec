using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpec
{
    public static class BeforeFinder
    {
        public static Context GetContexts(this Type type, Context childContext=null)
        {
            if (type.BaseType == typeof(spec))
            {
                var context = new Context( type );

                if(childContext!=null) context.AddContext(childContext);

                return context;
            }

            return GetContexts(type.BaseType, new Context(type));
        }

        public static  IEnumerable<Action<object>> GetBefores(this Type type) 
        {
            if (type.BaseType == typeof(spec)) return new[] { GetBefore(type) };

            return GetBefores(type.BaseType).Concat(new[] { GetBefore(type) });
        }

        public static Action<object> GetBefore(this Type type)
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