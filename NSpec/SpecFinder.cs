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
            return Types.Where(t => t.IsClass && BaseTypes(t).Any(s => s == typeof(spec)) && t.Methods(except).Count()>0);
        }

        public IEnumerable<Type> BaseTypes(Type type)
        {
            var types = new List<Type>();

            var currentType = type.BaseType;

            while(currentType != null)
            {
                types.Add(currentType);
                currentType = currentType.BaseType;
            }

            return types;
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

        private void RunSpecClass(Type specClass)
        {
            var spec = specClass.GetConstructors()[0].Invoke(new object[0]) as spec;

            var thisContext = new Context(specClass.Name);

            Contexts.Add(RootContext(spec, specClass, thisContext));

            specClass.Methods(except).Do(contextMethod =>
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

        public IEnumerable<Example> Examples()
        {
            return Contexts.SelectMany(c => c.AllExamples());
        }

        public IEnumerable<Example> Failures()
        {
            return Contexts.SelectMany(c => c.Failures());
        }

        public SpecFinder(string specDLL, IReflector reflector) : this()
        {
            this.reflector = reflector;

            Types = reflector.GetTypesFrom(specDLL);
        }

        public SpecFinder(params Type []types) : this()
        {
            Types = types;
        }

        public SpecFinder() 
        { 
            except = typeof(object).GetMethods().Select(m => m.Name).Union(new[] { "ClearExamples", "Examples", "set_Context", "get_Context" });

            Contexts = new List<Context>();
        }

        public IList<Context> Contexts { get; set; }

        private IEnumerable<string> except;
        private IReflector reflector;

        public Type[] Types { get; set; }
    }
}