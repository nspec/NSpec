using System;

namespace NSpec.Domain
{
    public class ContextRunner
    {
        public ContextRunner(ContextBuilder builder)
        {
            this.builder = builder;
        }

        public void Run()
        {
            var contexts = new ContextCollection();

            try
            {
                contexts = builder.Contexts();

                contexts.Do(c => c.Build());

                contexts.Do(c => c.Run());

                new ConsoleFormatter().Write(contexts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private ContextBuilder builder;
    }
}