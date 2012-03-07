using System;
using System.Reflection;

namespace NSpec.Domain
{
    public class Reflector : IReflector
    {
        public Type[] GetTypesFrom(string dll)
        {
            return Assembly.LoadFrom(dll).GetTypes();
        }

        public Type[] GetTypesFrom(Assembly assembly)
        {
            return assembly.GetTypes();
        }
    }

    public interface IReflector
    {
        Type[] GetTypesFrom(string dll);
        Type[] GetTypesFrom(Assembly assembly);
    }
}