using System;

namespace NSpec.Domain
{
    public static class BeforeFinder
    {
        public static Context RootContext(this Type type, Context childContext=null)
        {
            if (type.BaseType == typeof(nspec))
            {
                var context = new ClassContext( type );

                if(childContext!=null) context.AddContext(childContext);

                return context;
            }

            return RootContext(type.BaseType, new ClassContext(type));
        }
    }
}