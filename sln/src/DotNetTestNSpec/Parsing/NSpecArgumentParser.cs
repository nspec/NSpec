using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Parsing
{
    public class NSpecArgumentParser
    {
        public NSpecArgumentParser()
        {
            knownArgPrefixes = new[]
            {
                tagsKey,
                failFastKey,
                formatterPrefix,
                formatterOptionsPrefix,
            };
        }

        public NSpecCommandLineOptions Parse(string[] args)
        {
            IEnumerable<string> remainingArgs;

            // set default option values

            var options = new NSpecCommandLineOptions()
            {
                ClassName = null,
                Tags = null,
                FailFast = false,
                FormatterName = null,
                FormatterOptions = new Dictionary<string, string>(),
                UnknownArgs = new string[0],
            };

            // check for first argument (the class name), before remaining named options

            string firstArg = args.FirstOrDefault();

            if (!knownArgPrefixes.Contains(firstArg))
            {
                options.ClassName = firstArg;

                remainingArgs = args.Skip(1);
            }
            else
            {
                remainingArgs = args;
            }

            // check for remaining named options

            remainingArgs = ParsingUtils.SetTextForOptionalArg(remainingArgs,
                tagsKey, value => options.Tags = value);

            remainingArgs = SetOptionalFlag(remainingArgs,
                failFastKey, value => options.FailFast = value);

            remainingArgs = SetTextForOptionalPrefix(remainingArgs,
                formatterPrefix, value => options.FormatterName = value);

            int lastCount;

            do
            {
                lastCount = remainingArgs.Count();

                remainingArgs = SetTextForOptionalPrefix(remainingArgs,
                    formatterOptionsPrefix, text =>
                    {
                        string[] tokens = text.Split('=');

                        if (tokens.Length > 2)
                        {
                            throw new ArgumentException(
                                $"Formatter option '{text}' must be either a single 'flag' or a 'name=value' pair");
                        }

                        string name = tokens.First();
                        string value = tokens.Last();

                        options.FormatterOptions[name] = value;
                    });

            } while (lastCount != remainingArgs.Count());

            options.UnknownArgs = remainingArgs.ToArray();

            return options;
        }

        static IEnumerable<string> SetOptionalFlag(IEnumerable<string> args, string argKey, Action<bool> setValue)
        {
            bool hasKey = args.Contains(argKey);

            setValue(hasKey);

            return hasKey
                ? args.Where(arg => arg != argKey)
                : args;
        }

        static IEnumerable<string> SetTextForOptionalPrefix(IEnumerable<string> args, string argPrefix, Action<string> setValue)
        {
            string foundArg = args.FirstOrDefault(arg => arg.StartsWith(argPrefix, StringComparison.Ordinal));

            if (foundArg == null)
            {
                return args;
            }

            string[] tokens = foundArg.Split(new[] { argPrefix }, StringSplitOptions.RemoveEmptyEntries);

            string value = tokens.First();

            setValue(value);

            return args.Where(arg => arg != foundArg);
        }

        string[] knownArgPrefixes;

        const string tagsKey = "--tag";
        const string failFastKey = "--failfast";
        const string formatterPrefix = "--formatter=";
        const string formatterOptionsPrefix = "--formatterOptions:";
    }
}
