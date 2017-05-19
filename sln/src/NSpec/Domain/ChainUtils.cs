using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSpec.Domain
{
    public static class ChainUtils
    {
        public static List<MethodInfo> GetMethodsFromHierarchy(
            List<Type> classHierarchy, Func<Type, MethodInfo> selectMethod)
        {
            return classHierarchy
                .Select(selectMethod)
                .Where(m => m != null)
                .ToList();
        }
    }
}
