using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    public class ContextRunner
    {
        public ContextRunner(ContextBuilder builder, IFormatter formatter)
        {
            this.builder = builder;
            this.formatter = formatter;
        }

        public void Run()
        {
            var contexts = new ContextCollection();

            try
            {
                contexts = builder.Contexts();

                contexts.Build();

                ILiveFormatter liveFormatter = new SilentLiveFormatter();

                if (formatter is ConsoleFormatter) liveFormatter = formatter as ILiveFormatter;

                contexts.Run(liveFormatter);

                if (builder.tagsFilter.HasTagFilters()) contexts.TrimSkippedContexts();

                formatter.Write(contexts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private ContextBuilder builder;
        private IFormatter formatter;
    }
}