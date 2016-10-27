using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetTestNSpec
{
    public class ControllerProxy
    {
        public ControllerProxy(Assembly nspecLibraryAssembly)
        {
            controller = CreateController(nspecLibraryAssembly);
        }

        public int Run(
            string testAssemblyPath,
            string tags,
            string formatterClassName,
            IDictionary<string, string> formatterOptions,
            bool failFast)
        {
            object methodResult = ExecuteMethod(controller, runMethodName,
                testAssemblyPath, tags, formatterClassName, formatterOptions, failFast);

            int nrOfFailures = (int)methodResult;

            return nrOfFailures;
        }

        static object CreateController(Assembly nspecLibraryAssembly)
        {
            try
            {
                var typeInfo = nspecLibraryAssembly.DefinedTypes.Single(t => t.FullName == controllerTypeName);

                object controller = Activator.CreateInstance(typeInfo.AsType());

                return controller;
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(unknownDriverErrorMessage, ex);
            }
        }

        static object ExecuteMethod(object controller, string methodName, params object[] args)
        {
            var controllerType = controller.GetType();

            var methodInfo = controllerType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            if (methodInfo == null)
            {
                throw new DotNetTestNSpecException(unknownDriverErrorMessage);
            }

            object result = methodInfo.Invoke(controller, args);

            return result;
        }

        readonly object controller;

        const string controllerTypeName = "NSpec.Api.Controller";
        const string runMethodName = "Run";
        const string unknownDriverErrorMessage = "Could not find known driver or method in referenced NSpec library: please double check version compatibility between this runner and referenced NSpec.";
    }
}
