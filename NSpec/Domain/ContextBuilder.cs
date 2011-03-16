using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class ContextBuilder
    {
        public ContextBuilder(ISpecFinder finder)
        {
            this.finder = finder;
            Contexts = new List<Context>();
        }

        public void Run()
        {
            Execute(finder.SpecClasses());
        }

        public void Run(string classFilter)
        {
            if(finder.SpecClasses().Any(c => c.Name == classFilter))
                Execute(finder.SpecClasses().Where(c => c.Name == classFilter));
            else
                Run();
        }

        private void Build(Type specClass)
        {
            var root = specClass.RootContext();

            var classContext = root.SelfAndDescendants().First(c => c.Type == specClass);

            Contexts.Add(classContext);

            BuildMethodContexts(classContext, specClass);
        }

        public void BuildMethodContexts(Context classContext, Type specClass)
        {
            specClass.Methods(finder.Except).Do(contextMethod =>
            {
                var methodContext = new Context(contextMethod);

                classContext.AddContext(methodContext);
            });
        }

        private void Execute(IEnumerable<Type> specClasses)
        {
            Contexts.Clear();

            try
            {
                specClasses.Do(Build);

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

        private readonly ISpecFinder finder;
    }
}