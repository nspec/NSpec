using NSpec.Domain;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace NSpec.SourceInfo
{
    public static class ExampleUtils
    {
        public static MethodInfo GetBodyMethodInfo(ExampleBase baseExample)
        {
            string exampleTypeName = baseExample.GetType().Name;

            Func<ExampleBase, MethodInfo> getMethodInfo;

            if (nameof(Example) == exampleTypeName)
            {
                getMethodInfo = GetExampleBodyInfo;
            }
            else if (nameof(MethodExample) == exampleTypeName)
            {
                getMethodInfo = GetMethodExampleBodyInfo;
            }
            else if (nameof(AsyncExample) == exampleTypeName)
            {
                getMethodInfo = GetAsyncExampleBodyInfo;
            }
            else if (nameof(AsyncMethodExample) == exampleTypeName)
            {
                getMethodInfo = GetAsyncMethodExampleBodyInfo;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(baseExample), exampleTypeName, "Unknown example type");
            }

            return getMethodInfo(baseExample);
        }

        static MethodInfo GetExampleBodyInfo(ExampleBase baseExample)
        {
            // core logic taken from osoftware/NSpecTestAdapter:
            // see https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Discoverer.cs

            const string actionPrivateFieldName = "action";

            Example example = (Example)baseExample;

            var action = example.GetType()
                .GetField(actionPrivateFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(example) as Action;

            var info = action.GetMethodInfo();

            return info;
        }

        static MethodInfo GetMethodExampleBodyInfo(ExampleBase baseExample)
        {
            // core logic taken from osoftware/NSpecTestAdapter:
            // see https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Discoverer.cs

            const string methodInfoPrivateFieldName = "method";

            MethodExample example = (MethodExample)baseExample;

            var info = example.GetType()
                .GetField(methodInfoPrivateFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(example) as MethodInfo;

            return info;
        }

        static MethodInfo GetAsyncExampleBodyInfo(ExampleBase baseExample)
        {
            const string asyncActionPrivateFieldName = "asyncAction";

            AsyncExample example = (AsyncExample)baseExample;

            var asyncAction = example.GetType()
                .GetField(asyncActionPrivateFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(example) as Func<Task>;

            var info = asyncAction.GetMethodInfo();

            return info;
        }

        static MethodInfo GetAsyncMethodExampleBodyInfo(ExampleBase baseExample)
        {
            const string methodInfoPrivateFieldName = "method";

            AsyncMethodExample example = (AsyncMethodExample)baseExample;

            var info = example.GetType()
                .GetField(methodInfoPrivateFieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(example) as MethodInfo;

            return info;
        }
    }
}
