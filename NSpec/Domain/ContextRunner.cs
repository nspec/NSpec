using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class ContextRunner
    {
        private readonly IContextBuilder builder;

        public ContextRunner(IContextBuilder builder)
        {
            this.builder = builder;

            Contexts = new List<Context>();
        }

        public void Run()
        {
            Contexts.Clear();

            try
            {
                Contexts = builder.Contexts();

                Contexts.Do(c => c.Run());

                var formatter = new ConsoleFormatter();

                formatter.Write(Contexts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public IEnumerable<Example> Examples()
        {
            return Contexts.SelectMany(c => c.AllExamples());
        }

        public IEnumerable<Example> Failures()
        {
            return Contexts.SelectMany(c => c.Failures());
        }

        public IList<Context> Contexts{get;set;}
    }
}