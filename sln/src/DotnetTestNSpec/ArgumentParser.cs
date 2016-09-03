using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetTestNSpec
{
    public class ArgumentParser
    {
        public CommandLineOptions Parse(string[] args)
        {
            IEnumerable<string> dotNetTestArgs = args.TakeWhile(arg => arg != "--");
            IEnumerable<string> nSpecArgs = args.Skip(dotNetTestArgs.Count() + 1);

            var options = new CommandLineOptions();

            var remainingArgs = SetValueForOptionalArg(dotNetTestArgs,
                parentProcessArgKey, value => options.ParentProcessId = value);

            remainingArgs = SetValueForOptionalArg(remainingArgs,
                portArgKey, value => options.Port = value);

            options.NSpecArgs = nSpecArgs.ToArray();

            options.UnknownArgs = remainingArgs.ToArray();

            return options;
        }

        static IEnumerable<string> SetValueForOptionalArg(IEnumerable<string> args, string argKey, Action<int> setValue)
        {
            var argTail = args.SkipWhile(arg => arg != argKey);
            IEnumerable<string> unusedArgs;

            if (argTail.Any())
            {
                string potentialArgValue = argTail.Skip(1).FirstOrDefault();

                int value;
                bool argValueFound = Int32.TryParse(potentialArgValue, out value);

                if (!argValueFound)
                {
                    throw new ArgumentException($"Argument '{argKey}' must be followed by its value");
                }

                setValue(value);

                unusedArgs = args
                    .TakeWhile(arg => arg != argKey)
                    .Concat(argTail.Skip(2));
            }
            else
            {
                unusedArgs = args;
            }

            return unusedArgs;
        }

        const string parentProcessArgKey = "--parentProcessId";
        const string portArgKey = "--port";
    }
}
