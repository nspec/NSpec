using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Parsing
{
    public static class ParsingUtils
    {
        public static IEnumerable<string> SetTextForOptionalArg(IEnumerable<string> args, string argKey, Action<string> setValue)
        {
            var argTail = args.SkipWhile(arg => arg != argKey);
            IEnumerable<string> unusedArgs;

            if (argTail.Any())
            {
                string text = argTail.Skip(1).FirstOrDefault();

                setValue(text);

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
    }
}
