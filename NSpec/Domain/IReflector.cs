using System;
using System.Reflection;

namespace NSpec.Domain
{
    public class Reflector : IReflector
    {
        readonly string dll;

        public Reflector(string dll)
        {
            this.dll = dll;
        }

        public Type[] GetTypesFrom()
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
        Type[] GetTypesFrom();
        Type[] GetTypesFrom(Assembly assembly);
    }
}