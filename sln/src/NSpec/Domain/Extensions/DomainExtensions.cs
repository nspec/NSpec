using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NSpec.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static T CreateInstanceAs<T>(this Type type) where T : class
        {
            return type.GetTypeInfo().GetConstructors()[0].Invoke(new object[0]) as T;
        }

        public static IEnumerable<MethodInfo> Methods(this Type type)
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;

            var exclusions = typeof(nspec).GetTypeInfo().GetMethods(flags).Select(m => m.Name);

            var methodInfos = type.GetAbstractBaseClassChainWithClass().SelectMany(t => t.GetTypeInfo().GetMethods(flags));

            return methodInfos
                .Where(m => !exclusions.Contains(m.Name) && !(m.Name.Contains("<") || m.Name.Contains("$")) && m.Name.Contains("_"))
                .Where(m => m.GetParameters().Length == 0)
                .Where(m => !m.IsAsync())
                .Where(m => m.ReturnType == typeof(void)).ToList();
        }

        public static IEnumerable<MethodInfo> AsyncMethods(this Type type)
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;

            var exclusions = typeof(nspec).GetTypeInfo().GetMethods(flags).Select(m => m.Name);

            var methodInfos = type.GetAbstractBaseClassChainWithClass().SelectMany(t => t.GetTypeInfo().GetMethods(flags));

            return methodInfos
                .Where(m => !exclusions.Contains(m.Name) && !(m.Name.Contains("<") || m.Name.Contains("$")) && m.Name.Contains("_"))
                .Where(m => m.GetParameters().Length == 0)
                .Where(m => m.IsAsync()).ToList();
        }

        public static IEnumerable<Type> GetAbstractBaseClassChainWithClass(this Type type)
        {
            var baseClasses = new Stack<Type>();

            for (Type baseClass = type.GetTypeInfo().BaseType;
                 baseClass != null && baseClass.GetTypeInfo().IsAbstract;
                 baseClass = baseClass.GetTypeInfo().BaseType)
            {
                baseClasses.Push(baseClass);
            }

            while (baseClasses.Count > 0) yield return baseClasses.Pop();

            yield return type;
        }

        public static string CleanName(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType) return type.Name;

            return string.Format("{0}<{1}>", type.Name.Remove(type.Name.IndexOf('`')), string.Join(", ", type.GetTypeInfo().GetGenericArguments().Select(CleanName).ToArray()));
        }

        public static string CleanMessage(this Exception exception)
        {
            var exc = exception.Message.Trim().Replace("\r\n", ", ").Replace("\n", ", ").Trim();

            while (exc.Contains("  ")) exc = exc.Replace("  ", " ");

            return exc;
        }

        public static bool IsAsync(this MethodInfo method)
        {
            // Inspired from: https://github.com/nunit/nunit/blob/master/src/NUnitFramework/framework/Internal/AsyncInvocationRegion.cs

            return MethodReturnsTask(method) || MethodIsAsync(method);
        }

        private static bool MethodReturnsTask(MethodInfo method)
        {
            return method.ReturnType.FullName.StartsWith(TaskTypeName);
        }

        private static bool MethodIsAsync(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(false);

            bool? hasAsyncAttribute = attributes?
                .Any(attr => AsyncAttributeTypeName == attr.GetType().FullName);

            return hasAsyncAttribute
                ?? false;
        }

        public static bool IsAsync(this Action action)
        {
            return IsAsync(action.GetMethodInfo());
        }

        const string TaskTypeName = "System.Threading.Tasks.Task";
        const string AsyncAttributeTypeName = "System.Runtime.CompilerServices.AsyncStateMachineAttribute";
    }
}
