using NSpec.Domain;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.Api
{
    public class Controller
    {
        public int Run(
            string specAssemblyPath,
            string tags,
            string formatterClassName,
            IDictionary<string, string> formatterOptions,
            bool failFast)
        {
            var formatter = FindFormatter(formatterClassName, formatterOptions);

            var invocation = new RunnerInvocation(specAssemblyPath, tags, formatter, failFast);

            int nrOfFailures = invocation.Run().Failures().Count();

            return nrOfFailures;
        }

        /// <summary>
        /// Find an implementation of IFormatter with the given class name
        /// </summary>
        /// <param name="formatterClassName"></param>
        /// <param name="formatterOptions"></param>
        /// <returns></returns>
        private static IFormatter FindFormatter(string formatterClassName, IDictionary<string, string> formatterOptions)
        {
            // Default formatter is the standard console formatter
            if (string.IsNullOrEmpty(formatterClassName))
            {
                var consoleFormatter = new ConsoleFormatter();
                consoleFormatter.Options = formatterOptions;
                return consoleFormatter;
            }

            Assembly nspecAssembly = typeof(IFormatter).GetTypeInfo().Assembly;

            // Look for a class that implements IFormatter with the provided name
            var formatterType = nspecAssembly.GetTypes().FirstOrDefault(type =>
                (type.Name.ToLowerInvariant() == formatterClassName)
                && typeof(IFormatter).IsAssignableFrom(type));

            if (formatterType != null)
            {
                var formatter = (IFormatter)Activator.CreateInstance(formatterType);
                formatter.Options = formatterOptions;
                return formatter;
            }
            else
            {
                throw new TypeLoadException("Could not find formatter type " + formatterClassName);

            }
        }
    }
}
