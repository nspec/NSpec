using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSpec
{
    public class Finder
    {
        public Finder(string specDLL)
        {
            except = typeof(object).GetMethods().Select(m => m.Name).Union(new[] { "ClearExamples", "Examples", "SetContext" });

            Contexts = new List<Context>();

            Types = Assembly.LoadFrom(specDLL).GetTypes();
        }

        public IEnumerable<Type> SpecClasses()
        {
            return Types.Where(t => t.IsClass && t.BaseType == typeof (spec));
        }

        public void Run()
        {
            Contexts.Clear();

            SpecClasses().Do(RunSpecClass);

            Contexts.Where(c=>c.Examples.Count()>0 || c.Contexts.Count()>0).Do(e => e.Print());

            Console.WriteLine( string.Format("{0} Examples, {1} Failures", Examples().Count(), Failures().Count()));
        }

        private void RunSpecClass(Type specClass)
        {
            var spec = specClass.GetConstructors()[0].Invoke(new object[0]) as spec;

            specClass.Methods(except).Do(contextMethod =>
            {
                var context = new Context(contextMethod.Name);

                spec.SetContext( context );

                Contexts.Add(context);

                contextMethod.Invoke(spec, null);
            });
        }

        public IEnumerable<Example> Examples()
        {
            return Contexts.SelectMany(c => c.AllExamples());
        }

        public IEnumerable<Exception> Failures()
        {
            return Examples().Where(e => e.Exception != null).Select(e => e.Exception);
        }

        private IList<Context> Contexts { get; set; }
        private IEnumerable<string> except;
        private Type[] Types { get; set; }
    }
}