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

                if (Failures().Count() == 0)
                    Contexts.Where(c => c.Examples.Count() > 0 || c.Contexts.Count() > 0).Do(e => e.Print());
                else
                    Failures().First().Print();

                Summarize(Failures().Count());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                Summarize(Failures().Count() > 0 ? Failures().Count() : 1);
            }
        }

        private void Summarize(int failures)
        {
            Console.WriteLine( string.Format("{0} Examples, {1} Failures", Examples().Count(), failures));
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