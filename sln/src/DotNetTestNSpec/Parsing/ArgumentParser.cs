using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Parsing
{
    public class ArgumentParser
    {
        public ArgumentParser()
        {
            knownArgKeys = new[]
            {
                parentProcessArgKey,
                portArgKey,
            };
        }

        public CommandLineOptions Parse(string[] args)
        {
            IEnumerable<string> dotNetTestArgs = args.TakeWhile(arg => arg != "--");
            IEnumerable<string> nSpecArgs = args.Skip(dotNetTestArgs.Count() + 1);

            var options = new CommandLineOptions();

            // check for first argument (the project), before remaining dotnet-test options

            string firstArg = dotNetTestArgs.FirstOrDefault();

            if (!knownArgKeys.Contains(firstArg))
            {
                options.Project = firstArg;

                dotNetTestArgs = dotNetTestArgs.Skip(1);
            }

            // check for remaining dotnet-test options

            var remainingArgs = SetIntForOptionalArg(dotNetTestArgs,
                parentProcessArgKey, value => options.ParentProcessId = value);

            remainingArgs = SetIntForOptionalArg(remainingArgs,
                portArgKey, value => options.Port = value);

            options.NSpecArgs = nSpecArgs.ToArray();

            options.UnknownArgs = remainingArgs.ToArray();

            return options;
        }

        static IEnumerable<string> SetIntForOptionalArg(IEnumerable<string> args, string argKey, Action<int> setValue)
        {
            return ParsingUtils.SetTextForOptionalArg(args, argKey, text =>
            {
                int value;
                bool argValueFound = Int32.TryParse(text, out value);

                if (!argValueFound)
                {
                    throw new ArgumentException($"Argument '{argKey}' must be followed by its value");
                }

                setValue(value);
            });
        }

        string[] knownArgKeys;

        const string parentProcessArgKey = "--parentProcessId";
        const string portArgKey = "--port";
    }
}
