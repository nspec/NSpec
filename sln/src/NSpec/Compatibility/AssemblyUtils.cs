using System.IO;
using System.Reflection;

namespace NSpec.Compatibility
{
    public static class AssemblyUtils
    {
        public static Assembly LoadFromPath(string filePath)
        {
            Assembly assembly;

#if NET451
            assembly = Assembly.LoadFrom(filePath);
#else
            var assemblyName = Path.GetFileNameWithoutExtension(filePath);

            assembly = Assembly.Load(new AssemblyName(assemblyName));
#endif

            return assembly;
        }
    }
}
