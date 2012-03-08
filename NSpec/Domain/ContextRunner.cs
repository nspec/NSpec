using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    [Serializable]
    public class ContextRunner
    {
        public ContextCollection Run(ContextCollection contexts)
        {
            try
            {
                ILiveFormatter liveFormatter = new SilentLiveFormatter();

                if (formatter is ILiveFormatter) liveFormatter = formatter as ILiveFormatter;

                contexts.Run(liveFormatter, failFast);

                if (builder.tagsFilter.HasTagFilters()) contexts.TrimSkippedContexts();

                formatter.Write(contexts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return contexts;
        }

        public ContextRunner(ContextBuilder builder, IFormatter formatter, bool failFast)
        {
            this.failFast = failFast;
            this.builder = builder;
            this.formatter = formatter;
        }

        ContextBuilder builder;
        bool failFast;
        IFormatter formatter;
    }
}