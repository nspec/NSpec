using System;
using System.Linq;
using System.Reflection;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpec.Domain
{
    public static class BeforeFinder
    {
        public static Context RootContext(this Type type, Context childContext=null)
        {
            if (type.BaseType == typeof(spec))
            {
                var context = new Context( type );

                if(childContext!=null) context.AddContext(childContext);

                return context;
            }

            return RootContext(type.BaseType, new Context(type));
        }

        public static Action<object> GetBefore(this Type type)
        {
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            before<object> beforeEach = null;

            var instance = type.Instance<spec>();

            var eachField = fields.FirstOrDefault(f => f.Name.Contains("each"));

            if (eachField != null) beforeEach = eachField.GetValue(instance) as before<object>;

            if (beforeEach != null) return t => beforeEach(t);

            return null;
        }
    }
}