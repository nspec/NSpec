using System;
using System.IO;
using System.Reflection;

namespace NSpecRunner
{
    public class NSpecDomain
    {
        //largely inspired from:
        //http://thevalerios.net/matt/2008/06/run-anonymous-methods-in-another-appdomain/

        public NSpecDomain(string config)
        {
            this.config = config;
        }

        public void Run(string dll, string filter, Action<string, string> action)
        {
            var setup = new AppDomainSetup();

            setup.ConfigurationFile = Path.GetFullPath(config);

            setup.ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var domain = AppDomain.CreateDomain("NSpecDomain.Run", null, setup);

            var type = typeof(Wrapper);

            var assemblyName = type.Assembly.GetName().Name;

            var typeName = type.FullName;

            var wrapper = (Wrapper)domain.CreateInstanceAndUnwrap(assemblyName, typeName);

            wrapper.Execute(dll, filter, action);

            AppDomain.Unload(domain);
        }
        string config;
    }
}