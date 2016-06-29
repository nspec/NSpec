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
                Console.WriteLine("Exception executing method-level context: {0}".With(FullContext()));

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

            string exampleName = "Method context body throws an exception of type {0}".With(reportedEx.GetType().Name);

            // TODO create a specific exception type (e.g. BareCodeException) wrapping original reported exception
            // specific exception should explain more accurately to user what happened

            instance.it[exampleName] = () => { throw reportedEx; };
        }

        MethodInfo method;
    }
}
