using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    public class ContextRunner
    {
        public ContextCollection Run(ContextCollection contexts)
        {
            try
            {
                ILiveFormatter liveFormatter = new SilentLiveFormatter();

                if (formatter is ILiveFormatter) liveFormatter = formatter as ILiveFormatter;

                contexts.Run(liveFormatter, failFast);

                if (tagsFilter.HasTagFilters()) contexts.TrimSkippedContexts();

                formatter.Write(contexts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return contexts;
        }

        public ContextRunner(Tags tagsFilter, IFormatter formatter, bool failFast)
        {
            this.failFast = failFast;
            this.tagsFilter = tagsFilter;
            this.formatter = formatter;
        }

        Tags tagsFilter;
        bool failFast;
        IFormatter formatter;
    }
}