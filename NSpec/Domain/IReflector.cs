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
    }

    public interface IReflector
    {
        Type[] GetTypesFrom(string dll);
    }
}