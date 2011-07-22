using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSpec.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static T Instance<T>(this Type type) where T : class
        {
            return type.GetConstructors()[0].Invoke(new object[0]) as T;
        }

        public static IEnumerable<MethodInfo> Methods(this Type type)
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;

            var exclusions = typeof(nspec).GetMethods(flags).Select(m => m.Name);

            var methodInfos = type.GetMethods(flags);
            return methodInfos
                .Where(m => !exclusions.Contains(m.Name) && !m.Name.Contains("<") && m.Name.Contains("_"))
                .Where(m => m.GetParameters().Count() == 0)
                .Where(m => m.ReturnType.ToString() == "System.Void").ToList();
        }

        public static string CleanMessage(this Exception exception)
        {
            var exc = exception.Message.Trim().Replace(Environment.NewLine, ", ").Trim();

            while (exc.Contains("  ")) exc = exc.Replace("  ", " ");

            return exc;
        }

        public static void Each<T>( this IEnumerable<T> enumerable, Action<T> action )
        {
            foreach( var t in enumerable )
            {
                action( t );
            }
        }
    }
}