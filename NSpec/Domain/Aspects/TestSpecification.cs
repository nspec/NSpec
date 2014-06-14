using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSpec.Domain.Formatters;
using PostSharp.Aspects;

namespace NSpec.Domain.Aspects
{
    [Serializable]
    public class TestSpecification : MethodInterceptionAspect
    {
        private static List<MethodBase> methodsThatHaveBeenPrepared = new List<MethodBase>();


        // Invoked only once at runtime from the static constructor of type declaring the target method. 
        public override void RuntimeInitialize(MethodBase method)
        {
            string methodName = method.DeclaringType.FullName + method.Name;
        }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            if (methodsThatHaveBeenPrepared.Contains(args.Method))
            {
                base.OnInvoke(args);
                return;
            }

            methodsThatHaveBeenPrepared.Add(args.Method);

            var containingClass = args.Instance as nspec;
            var finder = new SingleClassGetter(args.Instance.GetType());
            var builder = new ContextBuilder(finder, new DefaultConventions());
            
            var runner = new ContextRunner(builder, new ConsoleFormatter(), false);

            var methodInfo = args.Method as MethodInfo;
            var contexts = builder.MethodContext(methodInfo);

            var contextCollection2 = contexts.Build();
            var results = runner.Run(contextCollection2);

            // if there were any failures, raise the exception so that the test framework has an error
            if (results.Failures().Any())
            {
                throw new TestFailedException();
            }

            /*
            // * get only the type for the current function
            var types = GetType().Assembly.GetTypes();

            // * a finder that only gives the current class
            var finder = new SpecFinder(types, "");
            var tags = new Tags().Parse("describe_ConsoleService");
            var convensions = new DefaultConventions();

            // * possibly a context builder that has a different set of arguments, the finder and function to invoke
            var builder = new ContextBuilder(finder, tags, convensions);

            // the output generator
            var formatter = new ConsoleFormatter();

            // the runner will stay the same
            var runner = new ContextRunner(builder, formatter, false);

            // * a MethodContext() function that takes the function name as parameter 
            var contextCollection = builder.Contexts();

            // still not sure what this does
            var contextCollection2 = contextCollection.Build();
            var results = runner.Run(contextCollection2);

            //results.Failures().Count().should_be(0);
             */
        }
    }
}
