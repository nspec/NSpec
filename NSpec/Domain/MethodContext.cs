using System;
using System.Reflection;

namespace NSpec.Domain
{
    public class MethodContext : Context
    {
        public override void Build(nspec instance)
        {
            base.Build(instance);

            try
            {
                method.Invoke(instance, null);
            }
            catch (Exception ex)
            {
                AddFailingExample(instance, ex);
            }
        }

        public MethodContext(MethodInfo method, string tags = null)
            : base(method.Name, tags)
        {
            this.method = method;
        }

        void AddFailingExample(nspec instance, Exception targetEx)
        {
            var reportedEx = (targetEx.InnerException != null)
                ? targetEx.InnerException
                : targetEx;

            string exampleName = "Method context body throws an exception of type '{0}'".With(reportedEx.GetType().Name);

            instance.it[exampleName] = () => { throw new ContextBareCodeException(reportedEx); };
        }

        MethodInfo method;
    }
}
