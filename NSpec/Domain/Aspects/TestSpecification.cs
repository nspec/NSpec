using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec.Domain.Formatters;
using PostSharp.Aspects;

namespace NSpec.Domain.Aspects
{
    [Serializable]
    public class TestSpecification : MethodInterceptionAspect
    {
        [XmlArrayItem(Type = typeof(MethodBase))]
        [XmlArray]
        private static List<MethodBase> methodsThatHaveBeenPrepared = new List<MethodBase>();

        // Invoked only once at runtime from the static constructor of type declaring the target method. 
        public override void RuntimeInitialize(MethodBase method)
        {
            string methodName = method.DeclaringType.FullName + method.Name;
        }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            // if the method has been prepared, we need to actually execute the function
            if (methodsThatHaveBeenPrepared.Contains(args.Method))
            {
                base.OnInvoke(args);
                return;
            }

            methodsThatHaveBeenPrepared.Add(args.Method);

            // prepare the npsec content of the function
            var finder = new SingleClassGetter(args.Instance.GetType());
            var builder = new ContextBuilder(finder, new DefaultConventions());
            var runner = new ContextRunner(builder, new ConsoleFormatter(), false);

            // set up the contexts for the method
            var methodInfo = args.Method as MethodInfo;
            var contexts = builder.MethodContext(methodInfo);

            // run the contexts
            var builtContexts = contexts.Build();
            var results = runner.Run(builtContexts);

            // if there were any failures, raise the exception so that the test framework has an error
            if (results.Failures().Any())
            {
                throw new TestFailedException();
            }

            // tests all passed
        }
    }
}
