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

        public IList<Context> Contexts{get;set;}

        public void Build()
        {
            finder.SpecClasses().Do(Run);
        }

        private void Run(Type specClass)
        {
            var spec = specClass.GetConstructors()[0].Invoke(new object[0]) as spec;

            var thisContext = new Context(specClass.Name);

            Contexts.Add(RootContext(spec, specClass, thisContext));

            specClass.Methods(finder.Except).Do(contextMethod =>
            {
                var context = new Context(contextMethod.Name);

                thisContext.AddContext(context);

                spec.Context = context;

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

        public Context RootContext(spec instance, Type specClass, Context instanceContext)
        {
            var rootContext = instanceContext;
            var rootType = specClass;

            rootContext.Before = GetBefore(rootContext, instance, instance.GetType());

            while (rootType.BaseType != typeof(spec))
            {
                var childSpec = rootContext;
                rootType = rootType.BaseType;
                rootContext = new Context(rootType.Name);
                rootContext.AddContext(childSpec);

                rootContext.Before = GetBefore(rootContext, instance, rootType);
            }

            return rootContext;
        }

        public Action GetBefore(Context context, spec instance, Type instanceType)
        {
            var fields = instanceType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            before<dynamic> beforeEach = null;

            foreach (FieldInfo field in fields)
            {
                if (field.Name.Contains("each"))
                {
                    beforeEach = (field.GetValue(instance) as before<dynamic>);

                    if (beforeEach != null)
                        break;
                }
            }

            if (beforeEach != null)
            {
                return () => beforeEach(instance);
            }

            return null;
        }

        public void Run()
        {
            Execute(finder.SpecClasses());
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


        public void Run(string class_filter)
        {
            if(finder.SpecClasses().Any(c => c.Name == class_filter))
                Execute(finder.SpecClasses().Where(c => c.Name == class_filter));
            else
                Run();
        }

        private readonly ISpecFinder finder;
    }
}