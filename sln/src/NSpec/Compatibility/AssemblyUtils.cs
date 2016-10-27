using System.IO;
using System.Reflection;

// TODO: turn all solution #if NETSTANDARD/NETCORE into opposite #if NET452

namespace NSpec.Compatibility
{
    public static class AssemblyUtils
    {
        public static Assembly LoadFromPath(string filePath)
        {
            Assembly assembly;

#if NETSTANDARD1_6
            var assemblyName = Path.GetFileNameWithoutExtension(filePath);

            assembly = Assembly.Load(new AssemblyName(assemblyName));
#else
            assembly = Assembly.LoadFrom(filePath);
#endif

            return assembly;
        }
    }
}
