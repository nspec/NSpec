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

        public static bool RunAndHandleException(Action<nspec> action, nspec instance, ref Exception exceptionToSet)
        {
            bool hasThrown = false;

            try
            {
                action(instance);
            }
            catch (TargetInvocationException invocationException)
            {
                if (exceptionToSet == null) exceptionToSet = instance.ExceptionToReturn(invocationException.InnerException);

                hasThrown = true;
            }
            catch (Exception exception)
            {
                if (exceptionToSet == null) exceptionToSet = instance.ExceptionToReturn(exception);

                hasThrown = true;
            }

            return hasThrown;
        }
    }
}
