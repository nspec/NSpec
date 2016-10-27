using System.IO;
using System.Reflection;

namespace DotNetTestNSpec.Compatibility
{
    public static class AssemblyUtils
    {
        public static string GetPrintInfo(this Assembly assembly)
        {
            string name = assembly.GetName().Name;

            var versionInfoAttribute = assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute))
                as AssemblyInformationalVersionAttribute;

            string version = versionInfoAttribute != null
                ? versionInfoAttribute.InformationalVersion
                : assembly.GetName().Version.ToString();

            string versionInfo = $"{name}: {version}";

            return versionInfo;
        }

        public static Assembly LoadFromPath(string filePath)
        {
            Assembly assembly;

#if NETCOREAPP1_0
            var assemblyName = Path.GetFileNameWithoutExtension(filePath);

            assembly = Assembly.Load(new AssemblyName(assemblyName));
#else
            assembly = Assembly.LoadFrom(filePath);
#endif

            return assembly;
        }
    }
}
