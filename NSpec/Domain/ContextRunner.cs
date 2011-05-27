using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    public class ContextRunner
    {
        public ContextRunner( ContextBuilder builder, IFormatter formatter )
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

                contexts.Do(c => c.Run());

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