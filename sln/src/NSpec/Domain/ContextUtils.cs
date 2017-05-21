using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSpec.Domain
{
    public static class ContextUtils
    {
        public static List<MethodInfo> GetMethodsFromHierarchy(
            List<Type> classHierarchy, Func<Type, MethodInfo> selectMethod)
        {
            return classHierarchy
                .Select(selectMethod)
                .Where(m => m != null)
                .ToList();
        }
        
        public static bool RunAndHandleException(Action<nspec> action, nspec instance, ref Exception targetException)
        {
            bool hasThrown = false;
            Exception exceptionToSet = null;

            try
            {
                action(instance);
            }
            catch (TargetInvocationException invocationException)
            {
                exceptionToSet = instance.ExceptionToReturn(invocationException.InnerException);

                hasThrown = true;
            }
            catch (Exception exception)
            {
                exceptionToSet = instance.ExceptionToReturn(exception);

                hasThrown = true;
            }

            if (targetException == null && exceptionToSet != null)
            {
                targetException = exceptionToSet;
            }

            return hasThrown;
        }
    }
}
