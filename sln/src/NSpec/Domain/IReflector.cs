using NSpec.Compatibility;
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
            var assembly = AssemblyUtils.LoadFromPath(dll);

            return assembly.GetTypes();
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