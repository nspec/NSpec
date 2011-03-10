using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpec.Domain
{
    public class ContextBuilder
    {
        public ContextBuilder(ISpecFinder finder)
        {
            this.finder = finder;
            Contexts = new List<Context>();
        }

        public void Build()
        {
            finder.SpecClasses().Do(Run);
        }

        public void Run()
        {
            Execute(finder.SpecClasses());
        }

        public void Run(string class_filter)
        {
            if(finder.SpecClasses().Any(c => c.Name == class_filter))
                Execute(finder.SpecClasses().Where(c => c.Name == class_filter));
            else
                Run();
        }

        private void Run(Type specClass)
        {
            Contexts.Add(specClass.GetContexts());

            var spec = specClass.Instance<spec>();

            Contexts.First().SetInstanceContext(spec);

            var classContext = Contexts.First().SelfAndDescendants().First(c => c.Type == specClass);

            specClass.Methods(finder.Except).Do(contextMethod =>
            {
                var methodContext = new Context(contextMethod.Name);

                classContext.AddContext(methodContext);

                spec.Context = methodContext;

                try
                {
                    contextMethod.Invoke(spec, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception executing context: {0}".With(methodContext.Name));

                    throw e;
                }
            });
        }

        private void Execute(IEnumerable<Type> specClasses)
        {
            Contexts.Clear();

            try
            {
                specClasses.Do(Run);

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

        private readonly ISpecFinder finder;
    }
}