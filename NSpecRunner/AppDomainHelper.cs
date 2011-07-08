using System;
using System.IO;
using System.Reflection;
using System.Security.Policy;

namespace ConsoleApplication1
{
    //http://thevalerios.net/matt/2008/06/run-anonymous-methods-in-another-appdomain/
    public static class AppDomainHelper
    {
        public static void ExecuteInNewAppDomain(Action method)
        {
            var setup = new AppDomainSetup();

            setup.ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var grantSet = PolicyLevel.CreateAppDomainLevel().GetNamedPermissionSet("FullTrust");

            var appDomain = AppDomain.CreateDomain("AppDomainHelper.ExecuteInNewAppDomain", null, setup, grantSet, null);

            var sandboxType = typeof(RemoteSandbox);

            var sandboxAssemblyName = sandboxType.Assembly.GetName().Name;

            var sandboxTypeName = sandboxType.FullName;

            var sandbox = (RemoteSandbox)appDomain.CreateInstanceAndUnwrap(sandboxAssemblyName, sandboxTypeName);

            sandbox.Execute(method);

            AppDomain.Unload(appDomain);
        }
        public static Tout ExecuteInNewAppDomain<Tin, Tout>(Tin input, Func<Tin, Tout> method)
        {
            var setup = new AppDomainSetup();

            setup.ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var grantSet = PolicyLevel.CreateAppDomainLevel().GetNamedPermissionSet("FullTrust");

            var appDomain = AppDomain.CreateDomain("AppDomainHelper.ExecuteInNewAppDomain", null, setup, grantSet, null);

            var sandboxType = typeof(RemoteSandbox<Tin, Tout>);

            var sandboxAssemblyName = sandboxType.Assembly.GetName().Name;

            var sandboxTypeName = sandboxType.FullName;

            var sandbox = (RemoteSandbox<Tin, Tout>)appDomain.CreateInstanceAndUnwrap(sandboxAssemblyName, sandboxTypeName);

            Tout output = sandbox.Execute(input, method);

            AppDomain.Unload(appDomain);

            return output;
        }
        public class RemoteSandbox : MarshalByRefObject
        {
            public void Execute(Action method)
            {
                method();
            }

            public override object InitializeLifetimeService()
            {
                return null;
            }
        }
        public class RemoteSandbox<Tin, Tout> : MarshalByRefObject
        {
            public Tout Execute(Tin input, Func<Tin, Tout> method)
            {
                return method(input);
            }

            public override object InitializeLifetimeService()
            {
                return null;
            }
        }
    }
}