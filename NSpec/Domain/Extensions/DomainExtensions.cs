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
            var flags = BindingFlags.Public | BindingFlags.Instance;// | BindingFlags.NonPublic;

            var exclusions = typeof(nspec).GetMethods(flags).Select(m => m.Name);

            return type.GetMethods(flags)
                .Where(m => !exclusions.Contains(m.Name))
                .Where(m =>m.GetParameters().Count()==0);
        }

        public static string CleanMessage(this Exception excpetion)
        {
            var exc = excpetion.Message.Trim().Replace(Environment.NewLine, ", ").Trim();

            while (exc.Contains("  ")) exc = exc.Replace("  ", " ");

            return exc;
        }
    }
}