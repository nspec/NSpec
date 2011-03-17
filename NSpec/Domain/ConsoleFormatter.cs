using System;
using System.Collections.Generic;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class ConsoleFormatter
    {
        public ConsoleFormatter()
        {
        }

        public string Write(Context context, int times=1)
        {
            var result = context.Name;

            context.Examples.Do(e => 
            {
                var failure = e.Exception == null ? "" : " - FAILED - {0}".With(e.Exception.Message.Replace(Environment.NewLine,", ").Trim());
                result += Environment.NewLine + "\t".Times(times) + e.Spec + failure;
            });

            context.Contexts.Do(c => result += Environment.NewLine + "\t".Times(times) + Write(c,times+1));

            return result;
        }

        public void Write(IList<Context> contexts)
        {
            contexts.Do( c=> Console.WriteLine(Write(c)));
        }
    }
}