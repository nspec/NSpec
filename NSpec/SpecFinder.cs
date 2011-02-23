using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Domain;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpec
{
    public class SpecFinder
    {
        public IEnumerable<Type> SpecClasses()
        {
            return Types.Where(t => t.IsClass && t.BaseType == typeof (spec));
        }

        public void Run()
        {
            Execute(SpecClasses());
        }

        private void Execute(IEnumerable<Type> specClasses)
        {
            Contexts.Clear();

            try
            {
                specClasses.Do(RunSpecClass);

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

        public void Run(string class_filter)
        {
            if(SpecClasses().Any(c => c.Name == class_filter))
                Execute(SpecClasses().Where(c => c.Name == class_filter));
            else
                Run();
        }

        private void RunSpecClass(Type specClass)
        {
            var spec = specClass.GetConstructors()[0].Invoke(new object[0]) as spec;

            specClass.Methods(except).Do(contextMethod =>
            {
                var context = new Context(contextMethod.Name);

                spec.Context = context;

                Contexts.Add(context);

                try
                {
                    contextMethod.Invoke(spec, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception executing context: {0}".With(context.Name));

                    throw e;
                }
            });
        }

        public IEnumerable<Example> Examples()
        {
            return Contexts.SelectMany(c => c.AllExamples());
        }

        public IEnumerable<Example> Failures()
        {
            return Contexts.SelectMany(c => c.Failures());
        }

        public SpecFinder(string specDLL)
        {
            except = typeof(object).GetMethods().Select(m => m.Name).Union(new[] { "ClearExamples", "Examples", "set_Context","get_Context" });

            Contexts = new List<Context>();

            Types = Assembly.LoadFrom(specDLL).GetTypes();
        }

        //public SpecFinder() : this(@"C:\Users\matt\Documents\Visual Studio 2010\Projects\NSpec\SampleSpecs\bin\Debug\SampleSpecs.dll") { }

        public SpecFinder() : this(@"C:\Users\matt\Documents\Visual Studio 2010\Projects\NSpec\NSpecSpec\bin\Debug\NSpecSpec.dll") { }

        private IList<Context> Contexts { get; set; }

        private IEnumerable<string> except;

        private Type[] Types { get; set; }
    }
}